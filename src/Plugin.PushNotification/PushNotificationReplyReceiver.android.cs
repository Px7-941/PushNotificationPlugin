﻿using System.Collections.Generic;
using Android.App;
using Android.Content;

namespace Plugin.PushNotification
{
    [BroadcastReceiver]
    public class PushNotificationReplyReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            var extras = intent?.Extras;

            if (extras != null && !extras.IsEmpty)
            {
                foreach (var key in extras.KeySet())
                {
                    parameters.Add(key, $"{extras.Get(key)}");
                }
            }
            var reply = RemoteInput.GetResultsFromIntent(intent)?.GetString("Result");
            PushNotificationManager.RegisterAction(parameters, reply);

            var manager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            var notificationId = extras.GetInt(DefaultPushNotificationHandler.ActionNotificationIdKey, -1);
            if (notificationId != -1)
            {
                var notificationTag = extras.GetString(DefaultPushNotificationHandler.ActionNotificationTagKey, string.Empty);

                if (notificationTag == null)
                {
                    manager.Cancel(notificationId);
                }
                else
                {
                    manager.Cancel(notificationTag, notificationId);
                }
            }
        }
    }
}
