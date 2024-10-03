namespace TestData
{
    public static class Constants
    {
        public static ICollection<string> PREFIX_LASTNAME_OPTIONS = new[] {
            "van", "van der", "van den", "van de", "de", "de la", "de van der", "der",
            "der van", "der van de", "den", "den van", "den de", "te", "ten", "ter",
            "in 't", "in 't veld", "in 't groen", "op", "op de", "op den", "op 't",
            "uit", "uit den", "uit de", "uit het", "aan", "aan de", "aan den",
            "aan het", "bij", "bij de", "bij den", "bij het", "onder", "onder de",
            "onder den", "onder het", "over", "over de", "over den", "over het",
            "van 't", "van 't hof", "van 't veld"
        };

        public static ICollection<string> DRIVERSLICENCE_CATEGORIES = new[]
        {
            "A1", "A2", "A",
            "B", "BE",
            "C1", "C1E", "C", "CE",
            "D1", "D1E", "D", "DE",
            "L", "T"
        };

        public static ICollection<string> DRIVERSLICENCE_CATEGORIES_DESCRIPTIONS = new[]
        {
            "Motor tot 125cc",    // A1
            "Motor tot 35kW",     // A2
            "Zware motor",        // A
            "Auto",               // B
            "Auto met aanhanger", // BE
            "Kleine vrachtwagen", // C1
            "Kleine vrachtwagen met aanhanger", // C1E
            "Vrachtwagen",        // C
            "Vrachtwagen met aanhanger", // CE
            "Kleine bus",         // D1
            "Kleine bus met aanhanger", // D1E
            "Bus",                // D
            "Bus met aanhanger",  // DE
            "Landbouwvoertuigen", // L
            "Tractor"             // T
        };


        public static ICollection<string> MARITALSTATUSES_OPTIONS = new[]
        {
            "Ongetrouwd",
            "Getrouwd",
            "Gescheiden",
            "Weduwe",
            "Weduwnaar"
        };

        public static ICollection<string> BENEFITFORM_OPTIONS = new[]
        {
            "Bijstandsuitkering",
            "Werkloosheidsuitkering",
            "Ziektewetuitkering",
            "Pensioen",
            "Invaliditeitsuitkering"
        };

        public static ICollection<string> DIAGNOSIS_OPTIONS = new[]
        {
            "ADHD",
            "Autisme",
            "Dyslexie",
            "Dyscalculie",
            "Obsessieve-Compulsieve Stoornis",
            "Depressie",
            "Bipolaire Stoornis",
            "Angststoornis",
            "Tourette Syndroom",
            "Posttraumatische Stressstoornis",
            "Oppositionele-opstandige stoornis",
            "Schizofrenie"
        };
    }
}
