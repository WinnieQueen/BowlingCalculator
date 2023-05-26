namespace BowlingCalculator.Models {
    internal class Frame : IFrame {
        public virtual int RoundsLeft { get; private protected set; } = 2;
        public virtual int Score { get; private protected set; }
        public bool IsPlayable { get => _pinsLeft > 0 && RoundsLeft > 0; }

        private bool _playedOne;

        public bool WasStrike { get; private set; }
        public bool WasSpare { get; private set; }
        public int FirstHit { get; private set; } = -1;

        public virtual int LastHit { get; private protected set; }
        private protected int _pinsLeft { get; set; } = 10;
        public virtual void HitPins(int pinsHit) {
            if (_pinsLeft <= 0 || RoundsLeft <= 0) {
                throw new ArgumentException($"Cannot hit anymore pins, this frame is finished!");
            }
            if (pinsHit > _pinsLeft) {
                throw new ArgumentException($"Cannot hit more than {_pinsLeft} pins");
            }
            else if (pinsHit < 0) {
                throw new ArgumentException("Cannot hit a negative number of pins");
            }

            _pinsLeft -= pinsHit;

            if (!_playedOne) {
                _playedOne = true;
                FirstHit = pinsHit;
                WasStrike = _pinsLeft == 0;
                RoundsLeft--;
            }
            else {
                WasSpare = _pinsLeft == 0;
                LastHit = pinsHit;
                RoundsLeft--;
            }
            Score += pinsHit;
        }

        public virtual string SecondLine() {
            var line = $"|{new String(' ', IFrame.TotalWidth - 6)}";
            if (WasStrike) {
                line += "|X|-|";
            }
            else if (WasSpare) {
                line += $"|{FirstHit}|/|";
            }
            else {
                line += $"|{FirstHit}|{LastHit}|";
            }
            return line;
        }
    }
}
