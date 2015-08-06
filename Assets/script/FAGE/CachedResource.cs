using UnityEngine;
using System.Collections.Generic;

public static class CachedResource {

	private	static Dictionary<string, Object> _cache = new Dictionary<string, Object>();

	public static T Load<T>(string path) where T : Object {
		if (!_cache.ContainsKey(path)) {
			_cache[path] = Resources.Load<T>(path);
		}
		return (T)_cache[path];
	}

	public	static void Clear() {
		_cache.Clear();
	}
}
