﻿using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;

/// <summary>
/// Tools for Pharo requests
/// </summary>
namespace PharoModule
{
    /// <summary>
    /// Sends Pharo requests to the server
    /// </summary>
    public static class Pharo
    {
        public static string IP = "http://localhost:1701/repl";
        public static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Starts a local Pharo server as a background task (EXPERIMENTAL)
        /// </summary>
        /// <returns></returns>
        public static async Task Start()
        {
            await Task.Delay(2000);
        }

        /// <summary>
        /// Sends a pice of code to execute
        /// </summary>
        /// <param name="code">The code</param>
        /// <returns>Response string</returns>
        public static async Task<string> Execute(string code)
        {
            var request = await client.PostAsync
            (
                IP, 
                new StringContent
                (
                    "[" + code + "]\n" +
                        "\ton: Exception\n" +
                        "\tdo: [:e | e traceCr].", 
                    Encoding.UTF8
                )
            );
            return await request.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Sends a pice of code to execute and print
        /// </summary>
        /// <param name="code">The code</param>
        /// <returns>Response string</returns>
        public static async Task<string> Print(string code)
        {
            if (!code.Contains("compile"))
                //code = "self class compiler evaluate: '" + code.Replace("'", "''") + "'";
                //code = "self class compiler evaluate: '" + code.Replace("''", "'").Replace("'", "''") + "'";
                code = "self class compiler evaluate: '" + Regex.Replace(code, @"([^'])'([^'])", "$1''$2") + "'";

            return await Execute(code);
        }

        /// <summary>
        /// Sends a pice of code to execute and inspect
        /// </summary>
        /// <param name="code">The code</param>
        /// <returns>Response string</returns>
        public static async Task<string> Inspect(string code)
        {
            return await Print
            (
                "| res tuples |\n" +
                "res := [" + code + "] value .\n" +
                "tuples := OrderedCollection new.\n" +
                "tuples addLast: 'self=',(res value asString).\n" +
                "res class instVarNames do: [ :each |\n" +
                    "\ttuples addLast: (each asString),'=', ((res instVarNamed: each value) asString).\n" +
                "].\n" +
                "tuples ."
            );
        }
    }
}
