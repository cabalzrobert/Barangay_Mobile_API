using System;
using System.Collections.Generic;
using Comm.Commons.Extensions;
using System.Linq;
using System.Globalization;

namespace webapi.App.Aggregates.Common
{
    public class NotificationDto
    {
        public static IEnumerable<dynamic> FilterNotifications(IEnumerable<dynamic> list, int limit = 50){
            if(list==null) return null;
            var items = Notifications(list);
            var count = items.Count();
            if(count>=limit){
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count-1).Concat(new[]{ o });
                filter.BaseFilter = o.DateTransaction;
            }
            return items;
        }
        public static IEnumerable<dynamic> Notifications(IEnumerable<dynamic> list){
            if(list==null) return null;
            return list.Select(e=> Notification(e));
        }

        public static IDictionary<string, object> Notification(IDictionary<string, object> data){
            dynamic o = Dynamic.Object;
            o.NotificationID = ((int)data["NOTIF_ID"].Str().ToDecimalDouble()).ToString("X");
            o.DateTransaction = data["RGS_TRN_TS"];
            o.Title = data["NOTIF_TTL"].Str();
            o.Description = data["NOTIF_DESC"].Str();
            o.IsCompany = data["S_COMP"].To<bool>(false);
            o.IsOpen = data["S_OPN"].To<bool>(false);
            o.CommunitytID = data["COMM_ID"].Str();
            o.PostID = data["POST_ID"].Str();
            o.CommentID = data["CMN_ID"].Str();
            o.ImgURL = data["IMG_URL"].Str();
            bool IsWinning = data["S_WNNG"].To<bool>(false);
            bool IsReceivedAmount = data["S_RCVNG_AMT"].To<bool>(false);
            if(IsWinning||IsReceivedAmount){
                if(IsWinning)o.IsWinning = IsWinning;
                else if(IsReceivedAmount)o.IsReceivedAmount = IsReceivedAmount;
                o.Amount = data["AMT"].Str().ToDecimalDouble();
            }
            string type = data["TYP"].Str();
            if(!type.IsEmpty()) o.Type = type;

            try{
                DateTime datetime = data["RGS_TRN_TS"].To<DateTime>();
                o.DateDisplay = datetime.ToString("MMM dd, yyyy");
                o.TimeDisplay = datetime.ToString("hh:mm:ss tt");
                o.FulldateDisplay = $"{o.DateDisplay} {o.TimeDisplay}";
            }catch{}
            return o;
        }


        public static IEnumerable<dynamic> FilterAnnouncements(IEnumerable<dynamic> list, int limit = 50)
        {
            if (list == null) return null;
            var items = Announcements(list);
            var count = items.Count();
            if (count >= limit) 
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                //filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> Announcements(IEnumerable<dynamic> list)
        {
            if (list == null) return null;
            return list.Select(e => Announcement(e));
        }

        public static IDictionary<string, object> Announcement(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.NotificationID = ((int)data["NOTIF_ID"].Str().ToDecimalDouble()).ToString("X");
            o.DateTransaction = data["RGS_TRN_TS"];
            o.Title = data["NOTIF_TTL"].Str();
            o.Description = data["NOTIF_DESC"].Str();
            o.IsCompany = data["S_COMP"].To<bool>(false);
            string strisopen = data["S_OPN"].Str();
            o.IsOpen = data["S_OPN"].To<bool>(false);
            o.PosterName = textInfo.ToUpper(textInfo.ToLower(data["FLL_NM"].Str())); 
            try
            {
                DateTime datetime = data["RGS_TRN_TS"].To<DateTime>();
                o.DateDisplay = datetime.ToString("MMM dd, yyyy");
                //o.TimeDisplay = datetime.ToString("hh:mm:ss tt");
                o.TimeDisplay = datetime.ToString("hh:mm tt");
                o.FulldateDisplay = $"{o.DateDisplay} {o.TimeDisplay}";
            }
            catch { }
            return o;
        }

    }

}