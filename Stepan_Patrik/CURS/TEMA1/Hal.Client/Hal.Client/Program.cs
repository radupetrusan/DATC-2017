using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{

    public enum UIState {
        UI_BREWERIES,
        UI_BEERS,
        UI_DETAILS,
        UI_NONE,
    };

    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            client.BaseAddress =  new Uri("http://datc-rest.azurewebsites.net/");
            client.DefaultRequestHeaders.Add("Accept", "application/hal+json"); 

            UIState gState = UIState.UI_NONE;
            List<Brewery> gBreweries = null;
            List<Beer> gBeers = null;

            string command = null;
            int currentBrewery = 0;
            int currentBeer = 0;
            
            while(true)
            {
                switch(gState)
                {
                    case UIState.UI_NONE:
                        {
                            gBreweries = Brewery.GetBrewriesRequest(client);
                            Brewery.PrintList(gBreweries);

                            // TODO: fix command reading !
                            try
                            {
                                command = Console.ReadLine();
                                currentBrewery = Int32.Parse(command);

                                gState = UIState.UI_BREWERIES;
                            }
                            catch { }
                            break;
                        }
                    case UIState.UI_BREWERIES:
                        {
                            // TODO: check out of bounds somewhere
                            var currentRes = gBreweries[currentBrewery].links.beers.href;

                            gBeers = Beer.GetBeersRequest(client, currentRes);

                            if(gBeers != null)
                                Beer.PrintList(gBeers);
                            else
                            {
                                gState = UIState.UI_NONE;
                                Console.WriteLine("Invalid resource!\n");
                                break;
                            }

                            command = Console.ReadLine();
                            try
                            {
                                currentBeer = Int32.Parse(command);
                                gState = UIState.UI_DETAILS;
                            }
                            catch
                            {
                                // Currently this serves as 'back' command also
                                gState = UIState.UI_NONE;
                            }
                            break;
                        }
                    case UIState.UI_DETAILS:
                        Beer.PrintDetails(gBeers[currentBeer]);

                        gState = UIState.UI_BREWERIES;
                        Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("State Error!\n");

                        break;
                }
            }

            Console.ReadLine();
        }
    }
}
