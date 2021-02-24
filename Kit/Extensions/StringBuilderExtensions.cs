﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Kit.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder TrimEnd(this StringBuilder sb)
        {
            int index = sb.Length - 1;
            if (index < 0) { return sb; }
            char last = sb[index];
            while (last == '\n' || last == '\r' && index > 0)
            {
                index = sb.Length - 1;
                if (index < 0) { return sb; }
                sb.Remove(index, 1);
                index--;
                last = sb[index];
            }
            return sb;
        }
        public static bool EndsWith(this StringBuilder sb, string test)
        {
            if (sb.Length < test.Length)
                return false;

            string end = sb.ToString(sb.Length - test.Length, test.Length);
            return end.Equals(test);
        }
    }
}