using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SVTTestProject.Models;

namespace SVTTestProject.Controllers;


[Route("/api/robots/closest")]
public class RobotSelectorController : Controller
{
    private List<Robot>? bots = new List<Robot>();
    private List<LoadAssignment> candidates = new List<LoadAssignment>();
    private bool firstDone = false;


    private static readonly int distanceRing = 10;
    private static readonly HttpClient robotClient = new HttpClient();
    private static readonly string robotListURL = "https://60c8ed887dafc90017ffbd56.mockapi.io/robots";

    [HttpPost]
    public async Task<string> Post([FromBody] Load Value)
    {
        Load load = new Load()
        {
            loadID = Value.loadID,
            x = Value.x,
            y = Value.y
        };

        //getRobots();
        bots = await callRobotList();

        // sort by distance to Load
        bots?.Sort((a, b) => CalculateDistance(a,load).CompareTo(CalculateDistance(b, load)));

        // make list of candidates who are the first or in the top 10 closest
        double distance = 0;

        foreach (var item in bots!)
        {
            distance = CalculateDistance(item, load);
            // only add candidates who have a distance <= 10 units
            if (distance <= distanceRing || !firstDone)
            {
                candidates.Add(MakeLoadAssignment(item, distance));
                firstDone = true;
            }
            else
            {
                // as soon as the first bot is > 10, then stop looking. Avoids looking at all of the bots
                break;
            }
        }

        // if more than 1 candidate, then sort by batteryLevel
        if(candidates.Count > 1)
        {
            candidates.Sort((a, b) => b.batteryLevel.CompareTo(a.batteryLevel));
        }

        // correct candidate is the first element.
        LoadAssignment selected = candidates[0];

        return JsonSerializer.Serialize(selected);
    }

    private Task<List<Robot>?> callRobotList()
    {
        return robotClient.GetFromJsonAsync<List<Robot>?>(robotListURL, new JsonSerializerOptions());
    }

    private double CalculateDistance(Robot bot, Load load)
    {
        return Math.Sqrt(Math.Pow(Math.Abs(bot.x - load.x), 2) + Math.Pow(Math.Abs(bot.y - load.y), 2));
    }

    private LoadAssignment MakeLoadAssignment(Robot bot, double distance)
    {
        return new LoadAssignment()
        {
            robotId = bot.robotId,
            batteryLevel = bot.batteryLevel,
            distanceToGoal = distance
        };
    }
}

