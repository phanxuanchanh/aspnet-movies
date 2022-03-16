using System;

namespace Common
{
    public class Rating
    {
        private long upvote;
        private long downvote;

        public Rating(long upvote, long downvote)
        {
            if (upvote < 0 || downvote < 0)
                throw new Exception("");

            this.upvote = upvote;
            this.downvote = downvote;
        }

        private double SovleUpvotePercent()
        {
            if (upvote == 0 && downvote == 0)
                return 0;
            return ((double)upvote / (upvote + downvote)) * 100;
        }

        public int SolveStar()
        {
            double upvotePercent = SovleUpvotePercent();

            if (upvotePercent == 100)
                return 10;
            else if (upvotePercent >= 90)
                return 9;
            else if (upvotePercent >= 80)
                return 8;
            else if (upvotePercent >= 70)
                return 7;
            else if (upvotePercent >= 60)
                return 6;
            else if (upvotePercent >= 50)
                return 5;
            else if (upvotePercent >= 40)
                return 4;
            else if (upvotePercent >= 30)
                return 3;
            else if (upvotePercent >= 20)
                return 2;
            else if (upvotePercent >= 10)
                return 1;
            else
                return 0;
        }

        public double SolveScore()
        { 
            return SovleUpvotePercent() / 10;
        }
    }
}
