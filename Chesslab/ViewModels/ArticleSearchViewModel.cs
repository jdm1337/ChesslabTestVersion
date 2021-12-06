using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Service;

namespace Chesslab.ViewModels
{
    public class ArticleSearchViewModel
    {
        public int ChosenCategory { get; set; }
        public int ChosenPeriod { get; set; }
        public async Task<List<int>> DefineChosenPeriodValue()
        {
            List<int> period = new List<int>();
            switch (ChosenPeriod)
            {
                case 1 :
                {
                    period.Add(0);
                    period.Add(2010);
                    break;
                    
                }
                case 2:
                {
                    period.Add(2010);
                    period.Add(2015);
                    break;

                }
                case 3:
                {
                    period.Add(2015);
                    period.Add(DateTime.Now.Year);
                    break;
                }
            }
            return period;
        }
    }
    
}
