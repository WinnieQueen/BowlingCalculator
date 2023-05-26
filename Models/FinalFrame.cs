namespace BowlingCalculator.Models {
    internal sealed class FinalFrame : Frame, IFrame {
        public override int RoundsLeft { get; private protected set; } = 3;
        public override int LastHit { get; private protected set; }
        public int BonusScore { get; private set; }

        private bool _bonusWasStrike;
        private bool _bonusWasDoubleStrike;
        private bool _bonusWasSpare;
        public override void HitPins(int pinsHit) {
            if (_pinsLeft <= 0 || RoundsLeft <= 0) {
                throw new ArgumentException($"Cannot hit anymore pins, this frame is finished!");
            }
            if (pinsHit > _pinsLeft) {
                throw new ArgumentException($"Cannot hit more than {_pinsLeft} pins");
            }
            else if (pinsHit < 0) {
                throw new ArgumentException("Cannot hit a negative number of pins");
            }

            switch (RoundsLeft) {
                case 3:
                    base.HitPins(pinsHit);
                    if (WasStrike) {
                        _pinsLeft = 10;
                    }
                    break;
                case 2:
                    LastHit = pinsHit;
                    if (WasStrike) { //if we got a strike in the first round, we can try for another strike, or start off a spare.
                        _pinsLeft -= pinsHit;
                        Score += pinsHit;
                        if (_bonusWasStrike = _pinsLeft == 0) { _pinsLeft = 10; }
                        RoundsLeft = 1; // either way, we get the third bonus round
                    }
                    else { //if we didn't get a strike in the first round, we can only try for a spare.
                        base.HitPins(pinsHit);
                        if (WasSpare) { //We get a final bonus round if we got a spare in this round
                            RoundsLeft = 1;
                            _pinsLeft = 10;
                        }
                        else {
                            RoundsLeft = 0;
                        }
                    }
                    break;
                case 1:
                    RoundsLeft--;
                    BonusScore = pinsHit; //We don't want this added to our full frame score, as that will affect the prior frame's scoring too!
                    if (_bonusWasStrike) { // if we got 2 strikes so far, we can try for another strike (score right now is X|X|?)
                        _pinsLeft -= pinsHit;
                        _bonusWasDoubleStrike = _pinsLeft == 0;
                    }
                    else {//if our bonus wasnt a strike, we can still try for the second half of a spare
                        _pinsLeft -= pinsHit;
                        _bonusWasSpare = _pinsLeft == 0;
                    }
                    break;
            }
        }

        public override string SecondLine() {
            var line = $"|{new String(' ', IFrame.TotalWidth - 8)}";
            if (WasSpare || WasStrike) {
                if (WasStrike) {
                    line += "|X|";
                }
                else if (WasSpare) {
                    line += $"|{FirstHit}|/|";
                }

                if (_bonusWasDoubleStrike) {
                    line += $"X|X|";
                }
                else if (_bonusWasStrike) {
                    line += $"X|{BonusScore}|";
                }
                else if (_bonusWasSpare) {
                    line += $"{LastHit}|/|";
                }
                else {
                    line += $"{LastHit}|{BonusScore}|";
                }
            }
            else {
                line += $"|{FirstHit}|{LastHit}|-|";
            }
            return line;
        }


    }
}
