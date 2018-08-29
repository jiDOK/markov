using System;
using System.Collections.Generic;

public class MarkovNameGenerator
{
    Dictionary<string, List<char>> chains = new Dictionary<string, List<char>>();
    List<string> samples = new List<string>();
    List<string> used = new List<string>();
    Random random = new Random();
    int order;
    int minLength;

    // sehr umfangreicher constructor
    // IEnumerable, damit wir foreach benutzen können
    // nimmt string[], List<string> etc.
    public MarkovNameGenerator(IEnumerable<string> sampleNames, int order, int minLength)
    {
        // clamp parameters
        if (order < 1) { order = 1; }
        if (minLength < 1) { minLength = 1; }
        this.order = order;
        this.minLength = minLength;

        // splitting and formatting
        foreach (string line in sampleNames)
        {
            string[] tokens = line.Split(',');
            foreach (string word in tokens)
            {
                string upper = word.Trim().ToUpper();
                if (upper.Length < order + 1)
                {
                    continue;
                }
                samples.Add(upper);
            }
        }

        // build chains
        foreach (string word in samples)
        {
            for (int letter = 0; letter < word.Length - order; letter++)
            {
                string token = word.Substring(letter, order);
                List<char> entry = null;
                if (chains.ContainsKey(token))
                {
                    entry = chains[token];
                }
                else
                {
                    entry = new List<char>();
                    chains[token] = entry;
                }
                entry.Add(word[letter + order]);
            }
        }
    }

    // sehr umfangreicher getter...
    public string NextName
    {
        get
        {
            string s = "";
            do
            {
                int n = random.Next(samples.Count);
                int nameLentgh = samples[n].Length;
                s = samples[n].Substring(random.Next(0, samples[n].Length - order), order);
                while (s.Length < nameLentgh)
                {
                    string token = s.Substring(s.Length - order, order);
                    char c = GetLetter(token);
                    if (c != '?') { s += c; }
                    else { break; }
                }

                if (s.Contains(" "))
                {
                    string[] tokens = s.Split(' ');
                    s = "";
                    for (int t = 0; t < tokens.Length; t++)
                    {
                        if (tokens[t] == "") { continue; }
                        // first letter upper case, rest lower case
                        else { tokens[t] = tokens[t].Substring(0, 1) + tokens[t].Substring(1).ToLower(); }
                        if (s != "") { s += " "; }
                        s += tokens[t];
                    }
                }
                // first letter upper case, rest lower case
                else { s = s.Substring(0, 1) + s.Substring(1).ToLower(); }
            }
            while (used.Contains(s) || s.Length < minLength);// wenn die Variation schon existiert, oder zu kurz: weitermachen
            used.Add(s);
            return s;
        }

    }
    // Reset the used names
    public void Reset()
    {
        used.Clear();
    }


    char GetLetter(string token)
    {
        // falls wir keinen Eintrag haben, können wir keinen zufälligen Nachfolger auslesen
        if (!chains.ContainsKey(token))
        {
            return '?';
        }
        List<char> letters = chains[token];
        int n = random.Next(letters.Count);
        return letters[n];
    }
}
