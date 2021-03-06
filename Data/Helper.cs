﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace PGC.Data {

    public static class Helper {
        public static IList<ItemData> GetDegrees () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (Degree item in Enum.GetValues (typeof (Degree))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

        public static IList<ItemData> GetRanks () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (Rank item in Enum.GetValues (typeof (Rank))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

        public static IList<ItemData> GetPositions () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (Position item in Enum.GetValues (typeof (Position))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

        public static IList<ItemData> GetStudyForms () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (StudyForm item in Enum.GetValues (typeof (StudyForm))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

        public static IList<ItemData> GetStatuses () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (StatusType item in Enum.GetValues (typeof (StatusType))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

        public static IList<ItemData> GetOrderTypes () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (OrderType item in Enum.GetValues (typeof (OrderType))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

        public static IEnumerable<ItemData> GetSpecialities (AspiranturaContext context) {
            // var specialities = context.Specialities.Where (t => t.IsUsed).Select (
            //     s => new ItemData {
            //         Value = s.Id,
            //             Text = new StringBuilder ()
            //             .Append (s.Id)
            //             .Append (new String (Convert.ToChar (160), 5 - s.Id.ToString ().Length))
            //             .ToString ()
            //     }).ToList ();

            IList<ItemData> list = new List<ItemData> ();
            var spec = context.Specialities.Where (t => t.IsUsed);
            foreach (var item in spec) {
                char ch = Convert.ToChar (160);
                string space = new String (ch, 5 - item.Id.ToString ().Length);
                string txt = item.Id + space + item.Name;
                list.Add (new ItemData () { Value = item.Id, Text = txt });
            }

            return list;
        }

        public static IEnumerable<ItemData> GetDepartments (AspiranturaContext context) {

            IList<ItemData> list = context.Departments.Select (d => new ItemData { Value = d.Id, Text = d.Acronym + " " + d.Name }).OrderBy (k => k.Text).ToList ();
            // var sp = context.Departments.Include ("Faculty");
            // foreach (var item in sp) {
            //     // char ch = Convert.ToChar (160);
            //     char ch = ' ';
            //     string spaceA = new String (ch, 8 - item.Acronym.ToString ().Length);
            //     string txt = item.Acronym + spaceA + item.Faculty.Acronym;
            //     list.Add (new ItemData () { Value = item.Id, Text = txt });
            // }

            return list;
        }

        public static IEnumerable<ItemData> GetPrepods (AspiranturaContext context) {
            //var prepods = context.Prepods.Where (t => t.Present).Select (s => new ItemData { Value = s.Id, Text = s.DepartmentsString + " " + s.FIO }).ToList ();
            var prepods = context.Prepods.Where (t => t.Present).Select (s => new ItemData { Value = s.Id, Text = s.FIO, Сomment = s.DepartmentsString }).OrderBy (p => p.Text).ToList ();
            return prepods;
        }

        // определить наличие целого слова в строке
        public static bool ContainsWholeWord (this string s, string word) {
            string pattern = @"\b" + Regex.Escape (word) + @"\b";
            // Regex re = new Regex (pattern, RegexOptions.IgnoreCase);
            //Regex re = new Regex (pattern);
            //var filtered = re.IsMatch (word);
            bool result = (Regex.Match (s, pattern).Success);
            return result;
        }
    }
}