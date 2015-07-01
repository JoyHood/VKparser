using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoyHoodApp
{
    [Flags]
    public enum Scope
    {
        /// <summary>
        /// Пользователь разрешил отправлять ему уведомления
        /// </summary>
        Notify = 1,

        /// <summary>
        /// Пользователь разрешил доступ к друзьям
        /// </summary>
        Friends = 2,

        /// <summary>
        /// Пользователь разрешил доступ к фото
        /// </summary>
        Photos = 4,

        /// <summary>
        /// Пользователь разрешил доступ к аудио
        /// </summary>
        Audio = 8,

        /// <summary>
        /// Доступ к видеозаписям.
        /// </summary>
        Video = 16,

        /// <summary>
        /// (для Standalone-приложений) Доступ к расширенным методам работы с сообщениями.
        /// </summary>
        Messages = 4096,

        /// <summary>
        /// Доступ к обычным и расширенным методам работы со стеной.
        /// </summary>
        Wall = 8192,

        /// <summary>
        /// Доступ к документам пользователя.
        /// </summary>
        Docs = 131072,

        /// <summary>
        /// Доступ к группам пользователя
        /// </summary>
        Groups = 262144
    }
}
