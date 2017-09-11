using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class NutritionReadings
    {
        public int ReadingId { get; set; }
        public double Carbohydrates { get; set; }
        public double Calories { get; set; }
        public double Protien { get; set; }
        public double Fat { get; set; }
        public DateTime ReadingDateTime { get; set; }
        public Guid ReadingKeyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }

        public ReadingHeaders ReadingKey { get; set; }
    }
}
