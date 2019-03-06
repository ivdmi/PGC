using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PGC.Data;

namespace PGC.Models.ViewModels {
    public class PrepodSelectLists {
        public PrepodSelectLists (Prepod prepod) {
            Positions = Helper.GetPositions ();
            Ranks = Helper.GetRanks ();
            Degrees = Helper.GetDegrees ();
            if (prepod != null) {
                selectedRank = Ranks.FirstOrDefault (k => k.Value == (int) prepod.Rank);
                selectedDegree = Degrees.FirstOrDefault (k => k.Value == (int) prepod.Degree);
                selectedPosition = Positions.FirstOrDefault (k => k.Value == (int) prepod.Position);
            }
        }

        public IEnumerable<ItemData> Ranks { get; set; }
        public IEnumerable<ItemData> Degrees { get; set; }
        public IEnumerable<ItemData> Positions { get; set; }

        // Это чтобы не выискивать в js - передадим готовое
        public ItemData selectedRank { get; set; }
        public ItemData selectedDegree { get; set; }
        public ItemData selectedPosition { get; set; }
    }
}