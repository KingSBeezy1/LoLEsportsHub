namespace LoLEsportsHub.Data.Common
{
    public static class EntityConstants
    {
        public static class Match
        {
            public const int TitleMaxLength = 100;

            public const int RegionMaxLength = 30;

            public const int VodUrlMaxLength = 2048;
        }

        public static class Tournament
        {
            public const int NameMaxLength = 80;
            public const int RegionMaxLength = 30;
        }

        public static class TournamentMatch
        {
            public const int AvailableSlotsDefaultValue = 0;
            public const string ScheduledTimeFormat = "{hh}:{mm}";
            public const int ScheduledTimeMaxLength = 50;
        }
    }
}
