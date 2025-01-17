using System.Reflection;

namespace PluginEngine.Message
{
    /// <summary>
    /// AppMessengerExtensions
    /// </summary>
    public static class AppMessengerExtensions
    {
        /// <summary>
        /// 注册类的静态消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Register<T>(this AppMessenger appMessenger) where T : IMessageObject
        {
            Type type = typeof(T);
            appMessenger.Register(type);
        }

        /// <summary>
        /// 注册类的静态消息
        /// </summary>
        /// <param name="appMessenger"></param>
        /// <param name="type"></param>
        /// <exception cref="NotSupportedException"></exception>
        public static void Register(this AppMessenger appMessenger, Type type)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Default);
            foreach (MethodInfo method in methods)
            {
                IEnumerable<Attribute> attributes = method.GetCustomAttributes();
                foreach (Attribute attribute in attributes)
                {
                    if (attribute is AppMessageAttribute att)
                    {
                        if (string.IsNullOrEmpty(att.Token))
                        {
                            appMessenger.Register(null, method.Name, method);
                        }
                        else
                        {
                            appMessenger.Register(null, att.Token, method);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="appMessenger"></param>
        /// <param name="messageObject"></param>
        public static void Register(this AppMessenger appMessenger, IMessageObject messageObject)
        {
            MethodInfo[] methods = messageObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Default);
            foreach (MethodInfo method in methods)
            {
                IEnumerable<Attribute> attributes = method.GetCustomAttributes();
                foreach (Attribute attribute in attributes)
                {
                    if (attribute is AppMessageAttribute att)
                    {
                        if (string.IsNullOrEmpty(att.Token))
                        {
                            appMessenger.Register(messageObject, method.Name, method);
                        }
                        else
                        {
                            appMessenger.Register(messageObject, att.Token, method);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="appMessenger"></param>
        /// <param name="messageObject"></param>
        /// <param name="token"></param>
        /// <param name="methodInfo"></param>
        /// <exception cref="MessageRegisteredException"></exception>
        public static void Register(this AppMessenger appMessenger, IMessageObject messageObject, string token, MethodInfo methodInfo)
        {
            appMessenger.Add(token, new MessageInstance(methodInfo, messageObject));
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register(this AppMessenger appMessenger, Action action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register<T>(this AppMessenger appMessenger, Action<T> action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register<T1, T2>(this AppMessenger appMessenger, Action<T1, T2> action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register<T1, T2, T3>(this AppMessenger appMessenger, Action<T1, T2, T3> action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register<T1, T2, T3, T4>(this AppMessenger appMessenger, Action<T1, T2, T3, T4> action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register<T1, T2, T3, T4, T5>(this AppMessenger appMessenger, Action<T1, T2, T3, T4, T5> action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register<T, TReturn>(this AppMessenger appMessenger, Func<T, TReturn> action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register<T1, T2, TReturn>(this AppMessenger appMessenger, Func<T1, T2, TReturn> action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register<T1, T2, T3, TReturn>(this AppMessenger appMessenger, Func<T1, T2, T3, TReturn> action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register<T1, T2, T3, T4, TReturn>(this AppMessenger appMessenger, Func<T1, T2, T3, T4, TReturn> action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register<T1, T2, T3, T4, T5, TReturn>(this AppMessenger appMessenger, Func<T1, T2, T3, T4, T5, TReturn> action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="appMessenger"></param>
        /// <param name="action"></param>
        /// <param name="token"></param>
        public static void Register<TReturn>(this AppMessenger appMessenger, Func<TReturn> action, string token = default)
        {
            appMessenger.RegisterDelegate(token, action);
        }

        /// <summary>
        /// 卸载消息
        /// </summary>
        /// <param name="appMessenger"></param>
        /// <param name="messageObject"></param>
        public static void Unregister(this AppMessenger appMessenger, IMessageObject messageObject)
        {
            appMessenger.Remove(messageObject);
        }

        /// <summary>
        /// 移除注册
        /// </summary>
        /// <param name="appMessenger"></param>
        /// <param name="token"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Unregister(this AppMessenger appMessenger, string token)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            appMessenger.Remove(token);
        }

        private static void RegisterDelegate(this AppMessenger appMessenger, string token, Delegate dele)
        {
            IEnumerable<Attribute> attributes = dele.Method.GetCustomAttributes();
            foreach (Attribute attribute in attributes)
            {
                if (attribute is AppMessageAttribute att)
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        if (string.IsNullOrEmpty(att.Token))
                        {
                            token = dele.Method.Name;
                        }
                        else
                        {
                            token = att.Token;
                        }
                    }

                    appMessenger.Add(token, new MessageInstance(dele.Method, dele.Target));
                }
            }
        }
    }
}