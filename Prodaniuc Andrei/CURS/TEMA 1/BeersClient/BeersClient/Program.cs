using BeersClient.Models;
using BeersClient.Services;
using BeersClient.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BeersClient
{
    class Program
    {
        private static HALClientService _service;
        private static string _userOption;
        private static List<Brewery> _breweries;
        static void Main(string[] args)
        {
            _breweries = new List<Brewery>();
            var brewery=new Brewery();
            _userOption = string.Empty;
            _service = new HALClientService();

            while (_userOption!="q")
            {
                if(_userOption == "b")
                {

                }
                else if (Regex.Matches(_userOption, "^[0-9]").Count>0)
                {
                    brewery = _breweries.FirstOrDefault(x => x.Id.ToString() == _userOption);
                    Menu.Display(_service.GetBrewery(brewery.Links.Self.Href));
                }
                else if(_userOption == "b")
                {
                    Menu.Display(_breweries);
                }
                else if (_userOption == "beers")
                {
                    var beers = brewery.Links.Beers.Href;
                    Menu.Display(_service.GetBeers(beers));
                }
                else
                {
                    _breweries = _service.GetBreweries();
                    Menu.Display(_breweries);

                }

                _userOption = Console.ReadLine();
            }
            Console.ReadKey();
        }
    }
}
