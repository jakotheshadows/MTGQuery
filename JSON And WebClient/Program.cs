using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace JSON_And_WebClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var wc = new WebClient())
            {
                
                string json = wc.DownloadString("http://mtgjson.com/json/AllCards.json");
                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = json.Length + 1;
                Dictionary<string, MTGCard> cards = serializer.Deserialize<Dictionary<string, MTGCard>>(json);
                while(true)
                {
                    Console.WriteLine("Enter your Magic: The Gathering card query or press enter to quit... ");
                    string cardQueryString = Console.ReadLine();
                    if(String.IsNullOrWhiteSpace(cardQueryString))
                        return;
                    IEnumerable<MTGCard> cardQuery = cards.Where(x => x.Key.ToLowerInvariant().Contains(cardQueryString)).Select(x => x.Value);
                    if(!cardQuery.Any())
                    {
                        Console.WriteLine("No such card was found");
                    }
                    else
                    {
                        cardQuery.ToList().ForEach(Console.WriteLine);
                    }
                }
            }
        }
    }

    public class MTGCard
    {
        /// <summary>
        /// Card Name
        /// </summary>
        public string name;
        /// <summary>
        /// Card mana cost
        /// </summary>
        public string manaCost;
        /// <summary>
        /// Card colors array
        /// </summary>
        public string[] colors;
        public string type;
        public string rarity;
        public string text;
        public string power;
        public string toughness;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(name + " : " + manaCost);
            if (colors == null)
                sb.AppendLine("Colorless");
            else
            {
                foreach (string s in colors)
                {
                    sb.Append(s + " ");
                }
            }
            sb.AppendLine();
            sb.AppendLine(type + " : " + rarity);
            sb.AppendLine(text);
            sb.AppendLine(power + " / " + toughness);
            return sb.ToString();
        }
    }
}
