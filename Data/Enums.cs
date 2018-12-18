using System;
using System.Collections.Generic;

namespace PGC.Data
{
    public enum OrderTypeEnum {
        Зарахування = 1,
        Переведення = 2,
        Закінчення = 3,
        Відрахування = 4,
        Академвідпустка = 5,
        Інший = 6,
        Зміна_форми_навчання = 7
    }

    public enum StatusTypeEnum {
        новий = 1,
        навчання = 2,
        академ_відпустка = 3,
        відрахований_2 = 4,
        захист_достроково = 5,
        закінчив = 6,
        повторний_курс = 7,
        стажування = 8
    }

    public class ItemData {
            public int Value { get; set; }
            public string Text { get; set; }
    }
public static class Helper
{
public static IList<ItemData> GetStatusList()
    {
            IList<ItemData> list = new List<ItemData> ();
            foreach (StatusTypeEnum item in Enum.GetValues (typeof (StatusTypeEnum))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.ToString () });
            }
            return list;
    }

public static IList<ItemData> GetOrderTypeList()
    {
            IList<ItemData> list = new List<ItemData> ();
            foreach (OrderTypeEnum item in Enum.GetValues (typeof (OrderTypeEnum))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.ToString () });
            }
            return list;
    }

}
    
}