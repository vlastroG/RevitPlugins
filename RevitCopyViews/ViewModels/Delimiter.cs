﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace RevitCopyViews.ViewModels {
    internal static class Delimiter {
        private static readonly char _value = '_';

        public static SplittedViewName SplitViewName(string originalName, SplitViewOptions splitViewOptions) {
            if(string.IsNullOrEmpty(originalName)) {
                throw new ArgumentException($"'{nameof(originalName)}' cannot be null or empty.", nameof(originalName));
            }

            if(splitViewOptions is null) {
                throw new ArgumentNullException(nameof(splitViewOptions));
            }

            if(originalName.StartsWith("{") && originalName.EndsWith("{")) {
                return new SplittedViewName() { ViewName = originalName };
            }

            string prefix = splitViewOptions.ReplacePrefix ? GetPrefix(originalName) : null;
            string suffix = splitViewOptions.ReplaceSuffix ? GetSuffix(originalName) : null;

            string elevation = GetElevation(originalName);
            string viewName = ReplaceName(originalName, prefix, suffix, elevation);
            if(string.IsNullOrEmpty(viewName)) {
                return new SplittedViewName() { ViewName = originalName };
            }

            return new SplittedViewName() { Prefix = Removes(prefix), ViewName = Removes(viewName), Suffix = Removes(suffix), Elevations = Removes(elevation) };
        }

        public static string CreateViewName(SplittedViewName splittedViewName) {
            var values = new[] { splittedViewName.Prefix, splittedViewName.ViewName, splittedViewName.Elevations, splittedViewName.Suffix };
            return string.Join(new string(new[] { _value }), values.Where(item => !string.IsNullOrEmpty(item)));
        }

        private static string Removes(string value) {
            return value?.Trim().Trim(_value);
        }

        private static string ReplaceName(string originamName, params string[] values) {
            foreach(var value in values.Where(item => !string.IsNullOrEmpty(item))) {
                originamName = originamName.Replace(value, string.Empty);
            }

            return originamName.Trim().Trim(_value);
        }

        private static string GetPrefix(string originalName) {
            int index = originalName.IndexOf(_value);
            return index == -1 ? null : originalName.Substring(0, index);
        }

        private static string GetSuffix(string originalName) {
            int index = originalName.LastIndexOf(_value);
            string suffix = index == -1 ? null : originalName.Substring(index, originalName.Length - index);

            string elevation = GetElevation(originalName);
            if(string.IsNullOrEmpty(elevation)) {
                return suffix;
            }

            return suffix.StartsWith(elevation) ? suffix.Replace(elevation, string.Empty) : suffix;
        }

        private static string GetElevation(string originalName) {
            return Regex.Match(originalName, $@"\d+.\d{{3}}").Value;
        }
    }

    internal class SplitViewOptions {
        public bool ReplacePrefix { get; set; }
        public bool ReplaceSuffix { get; set; }
    }

    internal class SplittedViewName {
        public string Prefix { get; set; }
        public string ViewName { get; set; }
        public string Elevations { get; set; }
        public string Suffix { get; set; }
    }
}