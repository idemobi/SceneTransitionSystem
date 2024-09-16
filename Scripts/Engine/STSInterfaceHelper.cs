using System;
using System.Collections.Generic;
using UnityEngine;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Provides helper methods for finding and retrieving interface-based components
    /// in Unity scenes.
    /// </summary>
    public static class STSInterfaceHelper
    {
        /// <summary>
        /// Maps interface types to their corresponding component types that inherit from
        /// or are assignable to the Component class in the Unity framework.
        /// </summary>
        /// <remarks>
        /// This dictionary is populated during initialization by scanning all types and
        /// identifying which types implement each interface. The key is the interface type,
        /// and the value is a list of component types that implement the interface.
        /// This mapping supports functionalities such as finding objects or components
        /// in a scene that match a specific interface.
        /// </remarks>
        private static Dictionary<Type, List<Type>> _interfaceToComponentMapping;

        /// <summary>
        /// Stores all types obtained from the current application domain's assemblies.
        /// This variable is used to cache the types and avoid repeated reflection calls,
        /// thereby improving performance when initializing interface to component mappings
        /// or finding inherited component types.
        /// </summary>
        private static Type[] _allTypes;

        /// <summary>
        /// Provides utility methods to aid in the handling and retrieval of Unity components
        /// that implement specific interfaces within the Scene Transition System (STS).
        /// </summary>
        static STSInterfaceHelper()
        {
            InitInterfaceToComponentMapping();
        }

        /// <summary>
        /// Initializes the interface to component mapping dictionary, which maps interfaces to their corresponding
        /// Unity component types. This method filters out system, Unity framework, and other non-relevant interfaces,
        /// then populates the dictionary with interfaces and lists of component types that implement these interfaces.
        /// </summary>
        private static void InitInterfaceToComponentMapping()
        {
            _interfaceToComponentMapping = new Dictionary<Type, List<Type>>();
            _allTypes = GetAllTypes();
            foreach (var curInterface in _allTypes)
            {
                if (!curInterface.IsInterface)
                {
                    continue;
                }

                var typeName = curInterface.ToString().ToLower();
                if (typeName.Contains("unity") || typeName.Contains("system.")
                                               || typeName.Contains("mono.") || typeName.Contains("mono.") || typeName.Contains("icsharpcode.")
                                               || typeName.Contains("nsubstitute") || typeName.Contains("nunit.") || typeName.Contains("microsoft.")
                                               || typeName.Contains("boo.") || typeName.Contains("serializ") || typeName.Contains("json")
                                               || typeName.Contains("log.") || typeName.Contains("logging") || typeName.Contains("test")
                                               || typeName.Contains("editor") || typeName.Contains("debug"))
                    continue;

                var typesInherited = GetTypesInheritedFromInterface(curInterface);

                if (typesInherited.Count <= 0)
                    continue;

                var componentsList = new List<Type>();

                foreach (var curType in typesInherited)
                {
                    if (curType.IsInterface)
                        continue;

                    if (!(typeof(Component) == curType || curType.IsSubclassOf(typeof(Component))))
                        continue;

                    if (!componentsList.Contains(curType))
                        componentsList.Add(curType);
                }

                _interfaceToComponentMapping.Add(curInterface, componentsList);
            }
        }

        /// <summary>
        /// Retrieves all the types from the current application domain.
        /// </summary>
        /// <returns>
        /// An array containing all the types defined in all assemblies loaded in the current application domain.
        /// </returns>
        private static Type[] GetAllTypes()
        {
            var res = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                res.AddRange(assembly.GetTypes());
            }

            return res.ToArray();
        }

        /// <summary>
        /// Retrieves all types that inherit from the specified interface type T.
        /// </summary>
        /// <typeparam name="T">The interface type to search for.</typeparam>
        /// <returns>An enumerable of types that inherit from the specified interface type T.</returns>
        private static IEnumerable<Type> GetTypesInheritedFromInterface<T>() where T : class
        {
            return GetTypesInheritedFromInterface(typeof(T));
        }

        /// <summary>
        /// Retrieves the list of types that inherit from a specified interface and are subclasses of <see cref="UnityEngine.Component"/>.
        /// </summary>
        /// <param name="type">The interface type to search for.</param>
        /// <returns>A list of types that inherit from the specified interface and are subclasses of <see cref="UnityEngine.Component"/>.</returns>
        private static IList<Type> GetTypesInheritedFromInterface(Type type)
        {
            if (null == _allTypes)
            {
                _allTypes = GetAllTypes();
            }

            var res = new List<Type>();

            foreach (var curType in _allTypes)
            {
                if (!(type.IsAssignableFrom(curType) && curType.IsSubclassOf(typeof(Component))))
                    continue;

                res.Add(curType);
            }

            return res;
        }

        /// <summary>
        /// Finds objects of a specified type in the scene.
        /// </summary>
        /// <typeparam name="T">The type of objects to find.</typeparam>
        /// <param name="firstOnly">If true, only the first object found is returned. Otherwise, all objects of the specified type are returned.</param>
        /// <returns>A list of objects of the specified type, or null if no objects were found.</returns>
        public static IList<T> FindObjects<T>(bool firstOnly = false) where T : class
        {
            var resList = new List<T>();

            var types = _interfaceToComponentMapping[typeof(T)];

            if (null == types || types.Count <= 0)
            {
                Debug.LogError("No descendants found for type " + typeof(T));
                return null;
            }

            foreach (var curType in types)
            {
                Object[] objects = firstOnly
                    ? new[] { Object.FindObjectOfType(curType) }
                    : Object.FindObjectsOfType(curType);

                if (null == objects || objects.Length <= 0)
                    continue;

                var tList = new List<T>();

                foreach (var curObj in objects)
                {
                    var curObjAsT = curObj as T;

                    if (null == curObjAsT)
                    {
                        Debug.LogError("Unable to cast '" + curObj.GetType() + "' to '" + typeof(T) + "'");
                        continue;
                    }

                    tList.Add(curObjAsT);
                }

                resList.AddRange(tList);
            }

            return resList;
        }

        /// <summary>
        /// Retrieves components from the given game object that implement the specified interface.
        /// </summary>
        /// <typeparam name="T">The type of the interface to search for.</typeparam>
        /// <param name="component">The component from which to start the search.</param>
        /// <param name="firstOnly">If true, only the first found component will be returned. If false, all matching components will be returned.</param>
        /// <returns>A list of components found that implement the specified interface.</returns>
        public static IList<T> GetInterfaceComponents<T>(this Component component, bool firstOnly = false) where T : class
        {
            var types = _interfaceToComponentMapping[typeof(T)];

            if (null == types || types.Count <= 0)
            {
                Debug.LogError("No descendants found for type " + typeof(T));
                return null;
            }

            var resList = new List<T>();

            foreach (var curType in types)
            {
                Component[] components = firstOnly
                    ? new[] { component.GetComponent(curType) }
                    : component.GetComponents(curType);

                if (null == components || components.Length <= 0)
                    continue;

                var tList = new List<T>();

                foreach (var curComp in components)
                {
                    var curCompAsT = curComp as T;

                    if (null == curCompAsT)
                    {
                        Debug.LogError("Unable to cast '" + curComp.GetType() + "' to '" + typeof(T) + "'");
                        continue;
                    }

                    tList.Add(curCompAsT);
                }

                resList.AddRange(tList);
            }

            return resList;
        }

        /// <summary>
        /// Retrieves the first component that implements the specified interface type from the given component.
        /// </summary>
        /// <typeparam name="T">The interface type to search for.</typeparam>
        /// <param name="component">The component from which to retrieve the interface implementation.</param>
        /// <returns>The first component that implements the specified interface type, or null if no such component is found.</returns>
        public static T GetInterfaceComponent<T>(this Component component) where T : class
        {
            var list = GetInterfaceComponents<T>(component, true);

            return list[0];
        }
    }
}