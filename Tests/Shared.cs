using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace HastyAPI.FreshBooks {
	public class Shared {
		public static string Username;
		public static string Token;

		public static void LoadAuth() {
			if(!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Token)) return; // already loaded

			var path = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\")).FullName;
			var cachepath = Path.Combine(path, "auth.cache");

			if(!File.Exists(cachepath)) {
				File.WriteAllText(cachepath, "username:token");
				throw new Exception("Please set auth info in " + path);
			}
			var cached = File.ReadAllText(cachepath).Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			if(cached.Length != 2) throw new Exception("auth.cache format is invalid, should be username:token");

			Username = cached[0];
			Token = cached[1];
		}

		public static FreshBooks NewCaller() {
			LoadAuth();
			return new FreshBooks(Username, Token);
		}
	}
}
