namespace BowlingCalculator.Models {
    internal interface IFrame {
        public abstract bool IsPlayable { get; }
        public abstract int RoundsLeft { get; }
        public abstract bool WasStrike { get; }
        public abstract bool WasSpare { get; }
        public abstract int FirstHit { get; }

        public abstract int Score { get; }

        public abstract void HitPins(int pinsHit);


        #region String Printing

        public const int TotalWidth = 10;
        public static string FirstLine() => $" {new string('_', TotalWidth - 2)} ";
        public abstract string SecondLine();
        public static string ThirdLine() => $"|{new string(' ', TotalWidth - 2)}|";
        public static string ScoreFourthLine(int score) {
            var numOfSpaces = (decimal)(TotalWidth - 2 - score.ToString().Length) / 2;
            var firstGap = (int)Math.Floor(numOfSpaces);
            var secondGap = (int)Math.Ceiling(numOfSpaces);
            return $"|{new string(' ', firstGap)}{score}{new string(' ', secondGap)}|";
        }
        public static string FifthFinalLine() => $"|{new string('_', TotalWidth - 2)}|";

        #endregion

    }
}
