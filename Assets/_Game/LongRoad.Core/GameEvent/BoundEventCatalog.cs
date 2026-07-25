using System;
using System.Collections.Generic;
using System.Reflection;

namespace LongRoad.Core.GameEvent
{
    public static class BoundEventCatalog
    {
        private static Dictionary<(BoundGameEventKind Kind, string Tag), List<Type>> _map;
        private static readonly IReadOnlyList<Type> Empty = Array.Empty<Type>();

        public static IReadOnlyList<Type> Get(BoundGameEventKind kind, string tag)
        {
            EnsureBuilt();

            if (string.IsNullOrWhiteSpace(tag))
                return Empty;

            var key = (kind, tag.Trim());
            return _map.TryGetValue(key, out var types) ? types : Empty;
        }

        public static bool HasAny(BoundGameEventKind kind, string tag)
        {
            return Get(kind, tag).Count > 0;
        }

        private static void EnsureBuilt()
        {
            if (_map != null)
                return;

            _map = new Dictionary<(BoundGameEventKind, string), List<Type>>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types;
                }

                if (types == null)
                    continue;

                foreach (var type in types)
                {
                    if (type == null || type.IsAbstract || !typeof(GameEventBase).IsAssignableFrom(type))
                        continue;

                    var attributes = type.GetCustomAttributes<BoundGameEventAttribute>();
                    foreach (var attribute in attributes)
                    {
                        if (string.IsNullOrEmpty(attribute.Tag))
                            continue;

                        var key = (attribute.Kind, attribute.Tag);
                        if (!_map.TryGetValue(key, out var list))
                        {
                            list = new List<Type>();
                            _map[key] = list;
                        }

                        list.Add(type);
                    }
                }
            }
        }
    }
}
