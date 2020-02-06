using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Console_Voting_Calculator
{

    class Rule
    {

        private string name;
        private float minPercent;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public float MinPercent
        {
            get { return minPercent; }
            set { minPercent = value; }
        }
    }

    class Country
    {

        private string name;
        private float percent;
        private byte agree = 0;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public float Percent
        { 
            get { return percent; }
            set { percent = value; }
        }

        public byte Agree
        {
            get { return agree; }
            set { agree = value; }
        }
    }

    class Program
    {
        
        //Reads a file and seperates each line by a comma
        static List<string[]> ReadFile(string path)
        {
            string[] lines = File.ReadAllLines(path);

            List<string[]> fileContents = new List<string[]>();

            string trimmedLine;
            string[] trimmedLineArray;

            foreach (string line  in lines)
            {
                trimmedLine = line.Remove(line.Length, 0);

                trimmedLineArray = trimmedLine.Split(',');

                fileContents.Add(trimmedLineArray);
            }

            return fileContents;

        }

        //Imports each of the countries as objects
        static List<Country> ImportCountries(List<string[]> fileContents)
        {

            List<Country> countries = new List<Country>();

            foreach (string[] line in fileContents)
            {

                Country tempCountry = new Country();

                tempCountry.Name = line[0];
                tempCountry.Percent = float.Parse(line[1]);
                countries.Add(tempCountry);
            }

            return countries;
        }

        //Imports each of the rules as objects
        static List<Rule> ImportRules(List<string[]> fileContents)
        {

            List<Rule> rules = new List<Rule>();

            foreach (string[] line in fileContents)
            {
                Rule tempRule = new Rule();

                tempRule.Name = line[0];
                tempRule.MinPercent = float.Parse(line[1]);
                rules.Add(tempRule);
            }

            return rules;
        }

        static void DisplayCountries(List<Country> countries)
        {
            foreach(Country country in countries)
            {
                Console.WriteLine(country.Name + " : " + country.Agree);
            }


        }

        static void DisplayRules (List<Rule> rules)
        {
            foreach(Rule rule in rules)
            {
                Console.WriteLine(rule.Name + " : " + rule.MinPercent);
            }

        }

        static List<Country> ChangeCountryVote(List<Country> countries, string countryToChange, string newVoteStatus)
        {

            foreach (Country country in countries)
            {
                if (country.Name == countryToChange)
                {

                    country.Agree = byte.Parse(newVoteStatus);
                    break;
                }
            }

            return countries;

        }

        static int[] GetCountryVotes(List<Country> countries)
        {

            int[] countryVotes = new int[3];

            foreach (Country country in countries)
            {
                if (country.Agree == 0) { countryVotes[0]++; }
                else if (country.Agree == 1) { countryVotes[1]++; }
                else { countryVotes[2]++; }
            }

            return countryVotes;


        }

        static float GetPercentForRule(List<Rule> rules,string ruleName)
        {

            float minPercentForRule = 0;

            foreach (Rule rule in rules)
            {

                if (rule.Name == ruleName)
                {

                    minPercentForRule = rule.MinPercent;
                    break;
                }
            }

            return minPercentForRule;
        }

        static void Main()
        {
            string countryPath = "C:\\Users\\joshu\\source\\repos\\Console Voting Calculator\\Console Voting Calculator\\eu.txt";
            List<Country> countries = ImportCountries(ReadFile(countryPath));

            string rulePath = "C:\\Users\\joshu\\source\\repos\\Console Voting Calculator\\Console Voting Calculator\\rules.txt";
            List<Rule> rules = ImportRules(ReadFile(rulePath));

            while (true)
            {
                Console.Write("> ");

                string command = Console.ReadLine();

                Console.WriteLine();

                if (command.Contains("show countries"))
                {

                    DisplayCountries(countries);

                } else if (command.Contains("show rules"))
                {

                    DisplayRules(rules);

                }

                else if (command.StartsWith("change"))
                {

                    string[] commandArray = command.Split(' ');

                    ChangeCountryVote(countries,commandArray[1],commandArray[3]);

      
      
                }

                else if (command.StartsWith("vote"))
                {

                    

                    float minPercentForRule = GetPercentForRule(rules,command.Remove(0,5));


                    int[] countryVotes = GetCountryVotes(countries);

                    Console.WriteLine("Percent for rule to be passed: " + minPercentForRule);
                    Console.WriteLine("Number of countries in agreement: " + countryVotes[0]);
                    Console.WriteLine("Number of countries not in agreement: " + countryVotes[1]);
                    Console.WriteLine("Number of countries who have abstained: " + countryVotes[2]);

                    int participatingCountries = countryVotes[0] + countryVotes[1];

                    if ((countryVotes[0] / participatingCountries) * 100 >= minPercentForRule)
                    {
                        Console.WriteLine("Final Result: Approved");
                    } 
                    else
                    {
                        Console.WriteLine("Final Result: Rejected");
                    }

                }

                Console.WriteLine();


            }

        }
    }
}
