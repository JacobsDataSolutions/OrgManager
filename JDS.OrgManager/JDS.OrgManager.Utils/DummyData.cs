// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using CsvHelper;
using JDS.OrgManager.Utils.Streets;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace JDS.OrgManager.Utils
{
    public static class DummyData
    {
        private static readonly string[] ignoreSuffixDirections = new[] { "IB", "OB", "SB", "NB", "EXPY", "LN" };

        private static readonly string[] ignoreSuffixes = new[] { "ER", "XR", "IB", "OB", "SB", "NB", "EXPY", "LN", " " };

        private static readonly string[] lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Wilson", "Martinez", "Anderson", "Taylor", "Thomas", "Hernandez", "Moore", "Martin", "Jackson", "Thompson", "White", "Lopez", "Lee", "Gonzalez", "Harris", "Clark", "Lewis", "Robinson", "Walker", "Perez", "Hall", "Young", "Allen", "Sanchez", "Wright", "King", "Scott", "Green", "Baker", "Adams", "Nelson", "Hill", "Ramirez", "Campbell", "Mitchell", "Roberts", "Carter", "Phillips", "Evans", "Turner", "Torres", "Parker", "Collins", "Edwards", "Stewart", "Flores", "Morris", "Nguyen", "Murphy", "Rivera", "Cook", "Rogers", "Morgan", "Peterson", "Cooper", "Reed", "Bailey", "Bell", "Gomez", "Kelly", "Howard", "Ward", "Cox", "Diaz", "Richardson", "Wood", "Watson", "Brooks", "Bennett", "Gray", "James", "Reyes", "Cruz", "Hughes", "Price", "Myers", "Long", "Foster", "Sanders", "Ross", "Morales", "Powell", "Sullivan", "Russell", "Ortiz", "Jenkins", "Gutierrez", "Perry", "Butler", "Barnes", "Fisher" };

        private static readonly string[] mensNames = new[] { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles", "Christopher", "Daniel", "Matthew", "Anthony", "Donald", "Mark", "Paul", "Steven", "Andrew", "Kenneth", "Joshua", "George", "Kevin", "Brian", "Edward", "Ronald", "Timothy", "Jason", "Jeffrey", "Ryan", "Jacob", "Gary", "Nicholas", "Eric", "Stephen", "Jonathan", "Larry", "Justin", "Scott", "Brandon", "Frank", "Benjamin", "Gregory", "Samuel", "Raymond", "Patrick", "Alexander", "Jack", "Dennis", "Jerry", "Tyler", "Aaron", "Jose", "Henry", "Douglas", "Adam", "Peter", "Nathan", "Zachary", "Walter", "Kyle", "Harold", "Carl", "Jeremy", "Keith", "Roger", "Gerald", "Ethan", "Arthur", "Terry", "Christian", "Sean", "Lawrence", "Austin", "Joe", "Noah", "Jesse", "Albert", "Bryan", "Billy", "Bruce", "Willie", "Jordan", "Dylan", "Alan", "Ralph", "Gabriel", "Roy", "Juan", "Wayne", "Eugene", "Logan", "Randy", "Louis", "Russell", "Vincent", "Philip", "Bobby", "Johnny", "Bradley" };

        private static readonly Random random = new Random();

        private static readonly Street[] streets;

        private static readonly string[] womensNames = new[] { "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan", "Jessica", "Sarah", "Karen", "Nancy", "Margaret", "Lisa", "Betty", "Dorothy", "Sandra", "Ashley", "Kimberly", "Donna", "Emily", "Michelle", "Carol", "Amanda", "Melissa", "Deborah", "Stephanie", "Rebecca", "Laura", "Sharon", "Cynthia", "Kathleen", "Helen", "Amy", "Shirley", "Angela", "Anna", "Brenda", "Pamela", "Nicole", "Ruth", "Katherine", "Samantha", "Christine", "Emma", "Catherine", "Debra", "Virginia", "Rachel", "Carolyn", "Janet", "Maria", "Heather", "Diane", "Julie", "Joyce", "Victoria", "Kelly", "Christina", "Joan", "Evelyn", "Lauren", "Judith", "Olivia", "Frances", "Martha", "Cheryl", "Megan", "Andrea", "Hannah", "Jacqueline", "Ann", "Jean", "Alice", "Kathryn", "Gloria", "Teresa", "Doris", "Sara", "Janice", "Julia", "Marie", "Madison", "Grace", "Judy", "Theresa", "Beverly", "Denise", "Marilyn", "Amber", "Danielle", "Abigail", "Brittany", "Rose", "Diana", "Natalie", "Sophia", "Alexis", "Lori", "Kayla", "Jane" };

        static DummyData()
        {
            using (var reader = new StreamReader(@"Streets\chicago-street-names.csv"))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<StreetClassMap>();
                var records = csv.GetRecords<Street>();
                streets = (from s in records where !ignoreSuffixes.Contains(s.Suffix) && !ignoreSuffixDirections.Contains(s.SuffixDirection) && !s.Name.Contains("RAMP") select s).ToArray();
            }
        }

        public static bool CoinToss() => random.Next() % 2 == 0;

        public static string GenerateFakeFirstOrMiddleName(bool male) => male ? mensNames[random.Next(mensNames.Length)] : womensNames[random.Next(womensNames.Length)];

        public static string GenerateFakeLastName() => lastNames[random.Next(lastNames.Length)];

        public static string GenerateFakeSSN()
        {
            var bytes = new byte[9];
            random.NextBytes(bytes);
            var span = new Span<byte>(bytes);
            for (var i = 0; i < 9; i++)
            {
                span[i] = (byte)(span[i] % 9 + 49);
            }
            return Encoding.ASCII.GetString(bytes).Insert(3, "-").Insert(6, "-");
        }

        public static DateTime GetRandomBirthDate() => new DateTime(1981, 1, 1).AddDays(random.Next(5475)).Date;

        public static (string, string) GetRandomChitownStreet()
        {
            var street = streets[random.Next(streets.Length)];
            var num = random.Next(street.MinAddress, street.MaxAddress + 1);
            var addr1 = $"{num} {street.FullName}";
            var addr2 = CoinToss() ? $"APT {random.Next(20) + 1}" : "";
            return (addr1, addr2);
        }

        public static string GetRandomChitownZip() => $"606{random.Next(100):00}-{random.Next(10000):0000}";

        public static DateTime GetRandomHireDate()
        {
            var companyFounded = new DateTime(2012, 1, 1);
            return companyFounded.AddDays(random.Next((DateTime.Today - companyFounded).Days));
        }

        public static DateTime GetRandomTerminationDate(DateTime startDate)
        {
            return startDate.AddDays(random.Next(90) + 90);
        }
    }
}