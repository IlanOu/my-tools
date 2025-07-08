using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = System.Random;

namespace Features.Name
{
    public static class NameGenerator
    {
        // Cache pour améliorer les performances
        private static readonly Dictionary<string, HashSet<string>> GeneratedNamesCache =
            new Dictionary<string, HashSet<string>>();

        // Structures phonétiques pour différentes cultures/styles
        private static readonly Dictionary<string, PhoneticStructure> Structures =
            new Dictionary<string, PhoneticStructure>
            {
                ["Fantasy"] = new PhoneticStructure
                {
                    Patterns = new[]
                    {
                        "CVCVCa", "VCCVlor", "CVCVndil", "ElCVC", "CVCVthir", "GalCVC", "CVCrion", "AelVC", "CVlVn",
                        "CVCVlis"
                    },
                    Consonants = "bcdflmnprstvz",
                    Vowels = "aeiouy",
                    SpecialConsonants = new Dictionary<char, string>
                    {
                        ['C'] = "bcdflmnprstvz",
                        ['G'] = "gkqx",
                        ['l'] = "l",
                        ['r'] = "r",
                        ['t'] = "t",
                        ['n'] = "n",
                        ['d'] = "d",
                        ['E'] = "E"
                    },
                    ForbiddenPatterns = new[] { "([^aeiou]{3})", "([aeiou]{3})", "(.)\\1\\1" },
                    Substitutions = new Dictionary<string, string>
                    {
                        ["cq"] = "c",
                        ["qc"] = "c",
                        ["dt"] = "t",
                        ["td"] = "d",
                        ["uo"] = "uo",
                        ["ji"] = "ji"
                    },
                    Mutations = new Dictionary<string, string[]>
                    {
                        ["th"] = new[] { "th" },
                        ["ch"] = new[] { "ch" },
                        ["sh"] = new[] { "sh" },
                        ["ph"] = new[] { "f" }
                    }
                },

                ["SciFi"] = new PhoneticStructure
                {
                    Patterns = new[]
                        { "CVx", "ZVCor", "CV-CV", "XVnCV", "CVCtrV", "VCto", "NeoCV", "CVCix", "TrVnCV", "CVCon" },
                    Consonants = "bcdfgklmnprstvz",
                    Vowels = "aeiou",
                    SpecialConsonants = new Dictionary<char, string>
                    {
                        ['C'] = "bcdfgklmnprstvz",
                        ['Z'] = "xz",
                        ['X'] = "xkq",
                        ['N'] = "N",
                        ['T'] = "T",
                        ['n'] = "n",
                        ['t'] = "t",
                        ['r'] = "r"
                    },
                    ForbiddenPatterns = new[] { "([^aeiou]{3})", "([aeiou]{3})", "(.)\\1\\1" },
                    Substitutions = new Dictionary<string, string>
                    {
                        ["cq"] = "c",
                        ["qc"] = "c",
                        ["dt"] = "t",
                        ["td"] = "d"
                    },
                    Mutations = new Dictionary<string, string[]>
                    {
                        ["tr"] = new[] { "tr" },
                        ["kr"] = new[] { "kr" },
                        ["vr"] = new[] { "vr" }
                    }
                },

                ["Human"] = new PhoneticStructure
                {
                    Patterns = new[]
                        { "CVCVn", "CVCey", "CVCia", "JVCob", "CVCVl", "CVCVck", "CVCVs", "CVCVm", "CVCVth", "CVCVr" },
                    Consonants = "bcdfghjklmnprstvw",
                    Vowels = "aeiouy",
                    SpecialConsonants = new Dictionary<char, string>
                    {
                        ['C'] = "bcdfghjklmnprstvw",
                        ['J'] = "jy",
                        ['n'] = "n",
                        ['l'] = "l",
                        ['c'] = "c",
                        ['k'] = "k",
                        ['s'] = "s",
                        ['b'] = "b",
                        ['m'] = "m",
                        ['r'] = "r",
                        ['t'] = "t",
                        ['h'] = "h"
                    },
                    ForbiddenPatterns = new[] { "([^aeiou]{3})", "([aeiou]{3})", "(.)\\1\\1", "([jqxz]{2})" },
                    Substitutions = new Dictionary<string, string>
                    {
                        ["cq"] = "c",
                        ["qc"] = "c",
                        ["dt"] = "t",
                        ["td"] = "d",
                        ["nm"] = "m",
                        ["mn"] = "n"
                    },
                    Mutations = new Dictionary<string, string[]>
                    {
                        ["th"] = new[] { "th" },
                        ["ch"] = new[] { "ch" },
                        ["sh"] = new[] { "sh" },
                        ["ph"] = new[] { "f" }
                    }
                }
            };

        // Génère un nom basé sur un style et des contraintes
        public static string Generate(string style = "Fantasy", int minLength = 4, int maxLength = 10, int? seed = null,
            bool ensureUnique = true)
        {
            // Initialiser le cache pour ce style si nécessaire
            if (ensureUnique && !GeneratedNamesCache.ContainsKey(style))
            {
                GeneratedNamesCache[style] = new HashSet<string>();
            }

            // Utiliser une graine spécifique si fournie
            Random rng = seed.HasValue ? new Random(seed.Value) : new Random();

            // Vérifier si le style existe
            if (!Structures.TryGetValue(style, out PhoneticStructure structure))
            {
                // Utiliser Fantasy par défaut
                structure = Structures["Fantasy"];
            }

            // Essayer de générer un nom qui respecte les contraintes
            for (int attempt = 0; attempt < 50; attempt++)
            {
                // Choisir un modèle aléatoire
                string pattern = structure.Patterns[rng.Next(structure.Patterns.Length)];

                // Générer le nom basé sur le modèle
                string name = GenerateFromPattern(pattern, structure, rng);

                // Appliquer les mutations phonétiques
                name = ApplyPhoneticMutations(name, structure, rng);

                // Vérifier et corriger les combinaisons interdites
                name = FixForbiddenPatterns(name, structure);

                // Ajuster la longueur si nécessaire
                if (name.Length < minLength)
                {
                    // Ajouter un suffixe
                    string suffix = GenerateFromPattern(structure.Patterns[rng.Next(structure.Patterns.Length)],
                        structure, rng);
                    suffix = suffix.ToLower();

                    // Éviter les répétitions de voyelles à la jonction
                    if (IsVowel(name[name.Length - 1]) && IsVowel(suffix[0]))
                    {
                        suffix = suffix.Substring(1);
                    }

                    name = name + suffix;
                }

                // Tronquer si trop long
                if (name.Length > maxLength)
                {
                    // Trouver un point de coupure naturel (après une voyelle si possible)
                    int cutPoint = maxLength;
                    while (cutPoint > minLength && !IsVowel(name[cutPoint - 1]) && !IsVowel(name[cutPoint - 2]))
                    {
                        cutPoint--;
                    }

                    name = name.Substring(0, cutPoint);
                }

                // Vérifier l'unicité si demandé
                if (!ensureUnique || !GeneratedNamesCache[style].Contains(name))
                {
                    // Vérifier la longueur finale
                    if (name.Length >= minLength && name.Length <= maxLength)
                    {
                        // Ajouter au cache si nécessaire
                        if (ensureUnique)
                        {
                            GeneratedNamesCache[style].Add(name);
                        }

                        return name;
                    }
                }
            }

            // Fallback si les tentatives échouent
            return "Nameless" + UnityEngine.Random.Range(1, 1000);
        }

        // Génère plusieurs noms uniques
        public static string[] GenerateMultiple(int count, string style = "Fantasy", int minLength = 4,
            int maxLength = 10, int? seed = null)
        {
            string[] names = new string[count];

            for (int i = 0; i < count; i++)
            {
                names[i] = Generate(style, minLength, maxLength, seed.HasValue ? seed.Value + i : null, true);
            }

            return names;
        }

        // Génère un nom basé sur un modèle phonétique
        private static string GenerateFromPattern(string pattern, PhoneticStructure structure, Random rng)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in pattern)
            {
                if (c == 'C')
                {
                    // Consonne générique
                    result.Append(GetRandomChar(structure.SpecialConsonants['C'], rng));
                }
                else if (c == 'V')
                {
                    // Voyelle
                    result.Append(GetRandomChar(structure.Vowels, rng));
                }
                else if (structure.SpecialConsonants.ContainsKey(c))
                {
                    // Consonne spécifique ou préfixe spécial
                    if (c == 'N')
                    {
                        result.Append("Neo");
                    }
                    else if (c == 'T')
                    {
                        result.Append("Tr");
                    }
                    else if (c == 'E')
                    {
                        result.Append(GetElficPrefix(rng));
                    }
                    else
                    {
                        result.Append(GetRandomChar(structure.SpecialConsonants[c], rng));
                    }
                }
                else
                {
                    // Caractère littéral
                    result.Append(c);
                }
            }

            // Capitaliser la première lettre
            string name = result.ToString();
            if (name.Length > 0)
            {
                name = char.ToUpper(name[0]) + name.Substring(1);
            }

            return name;
        }

        // Applique des mutations phonétiques pour plus de naturel
        private static string ApplyPhoneticMutations(string name, PhoneticStructure structure, Random rng)
        {
            // Appliquer les substitutions
            foreach (var sub in structure.Substitutions)
            {
                name = name.Replace(sub.Key, sub.Value);
            }

            // Appliquer les mutations
            foreach (var mutation in structure.Mutations)
            {
                if (name.Contains(mutation.Key) && rng.Next(100) < 80) // 80% de chance d'appliquer
                {
                    string replacement = mutation.Value[rng.Next(mutation.Value.Length)];
                    name = name.Replace(mutation.Key, replacement);
                }
            }

            return name;
        }

        // Corrige les motifs interdits
        private static string FixForbiddenPatterns(string name, PhoneticStructure structure)
        {
            foreach (string pattern in structure.ForbiddenPatterns)
            {
                Match match = Regex.Match(name, pattern);
                while (match.Success)
                {
                    string matched = match.Groups[1].Value;

                    // Simplifier les groupes de consonnes ou voyelles
                    if (matched.Length > 2)
                    {
                        string replacement = matched.Substring(0, 2);
                        name = name.Replace(matched, replacement);
                    }

                    // Éviter les triples répétitions
                    if (matched.Length == 3 && matched[0] == matched[1] && matched[1] == matched[2])
                    {
                        string replacement = matched.Substring(0, 2);
                        name = name.Replace(matched, replacement);
                    }

                    match = Regex.Match(name, pattern);
                }
            }

            return name;
        }

        // Vérifie si un caractère est une voyelle
        private static bool IsVowel(char c)
        {
            return "aeiouAEIOU".IndexOf(c) >= 0;
        }

        // Obtenir un caractère aléatoire d'une chaîne
        private static char GetRandomChar(string source, Random rng)
        {
            return source[rng.Next(source.Length)];
        }

        // Générer un préfixe elfique
        private static string GetElficPrefix(Random rng)
        {
            string[] prefixes =
            {
                "Ael", "Aer", "Af", "Ah", "Al", "Am", "Ama", "An", "Ang", "Ansr", "Ar", "Ari", "Arn", "Aza", "Bael",
                "Cael"
            };
            return prefixes[rng.Next(prefixes.Length)];
        }

        // Ajouter un nouveau style
        public static void AddStyle(string styleName, string[] patterns, string consonants, string vowels,
            Dictionary<char, string> specialConsonants = null,
            string[] forbiddenPatterns = null,
            Dictionary<string, string> substitutions = null,
            Dictionary<string, string[]> mutations = null)
        {
            Structures[styleName] = new PhoneticStructure
            {
                Patterns = patterns,
                Consonants = consonants,
                Vowels = vowels,
                SpecialConsonants = specialConsonants ?? new Dictionary<char, string> { ['C'] = consonants },
                ForbiddenPatterns = forbiddenPatterns ?? new[] { "([^aeiou]{3})", "([aeiou]{3})", "(.)\\1\\1" },
                Substitutions = substitutions ?? new Dictionary<string, string>(),
                Mutations = mutations ?? new Dictionary<string, string[]>()
            };

            // Initialiser le cache pour ce style
            GeneratedNamesCache[styleName] = new HashSet<string>();
        }

        // Vider le cache pour un style spécifique ou tous les styles
        public static void ClearCache(string style = null)
        {
            if (style != null)
            {
                if (GeneratedNamesCache.ContainsKey(style))
                {
                    GeneratedNamesCache[style].Clear();
                }
            }
            else
            {
                foreach (var key in GeneratedNamesCache.Keys.ToList())
                {
                    GeneratedNamesCache[key].Clear();
                }
            }
        }

        // Vérifier si un nom existe déjà dans le cache
        public static bool NameExists(string name, string style = null)
        {
            if (style != null)
            {
                return GeneratedNamesCache.ContainsKey(style) && GeneratedNamesCache[style].Contains(name);
            }
            else
            {
                foreach (var cache in GeneratedNamesCache.Values)
                {
                    if (cache.Contains(name))
                        return true;
                }

                return false;
            }
        }

        // Obtenir tous les noms générés pour un style
        public static string[] GetGeneratedNames(string style)
        {
            if (GeneratedNamesCache.ContainsKey(style))
            {
                return GeneratedNamesCache[style].ToArray();
            }

            return new string[0];
        }

        // Générer un nom basé sur un modèle spécifique
        public static string GenerateFromCustomPattern(string pattern, string consonants, string vowels,
            int? seed = null)
        {
            Random rng = seed.HasValue ? new Random(seed.Value) : new Random();

            PhoneticStructure tempStructure = new PhoneticStructure
            {
                Patterns = new[] { pattern },
                Consonants = consonants,
                Vowels = vowels,
                SpecialConsonants = new Dictionary<char, string> { ['C'] = consonants },
                ForbiddenPatterns = new[] { "([^aeiou]{3})", "([aeiou]{3})", "(.)\\1\\1" },
                Substitutions = new Dictionary<string, string>(),
                Mutations = new Dictionary<string, string[]>()
            };

            return GenerateFromPattern(pattern, tempStructure, rng);
        }

        // Fusionner deux noms pour créer un nom hybride
        public static string MergeNames(string name1, string name2, int? seed = null)
        {
            Random rng = seed.HasValue ? new Random(seed.Value) : new Random();

            // Déterminer les points de coupure
            int cut1 = rng.Next(name1.Length / 2, name1.Length);
            int cut2 = rng.Next(1, name2.Length / 2);

            // Créer le nom hybride
            string hybrid = name1.Substring(0, cut1) + name2.Substring(cut2);

            // Éviter les répétitions de voyelles ou consonnes à la jonction
            int joinPoint = cut1;
            if (joinPoint < hybrid.Length - 1)
            {
                if (IsVowel(hybrid[joinPoint - 1]) && IsVowel(hybrid[joinPoint]))
                {
                    hybrid = hybrid.Remove(joinPoint, 1);
                }
                else if (!IsVowel(hybrid[joinPoint - 1]) && !IsVowel(hybrid[joinPoint]) &&
                         !IsVowel(hybrid[joinPoint + 1]))
                {
                    // Insérer une voyelle entre trois consonnes
                    hybrid = hybrid.Insert(joinPoint, GetRandomChar("aeiou", rng).ToString());
                }
            }

            // Capitaliser la première lettre
            if (hybrid.Length > 0)
            {
                hybrid = char.ToUpper(hybrid[0]) + hybrid.Substring(1).ToLower();
            }

            return hybrid;
        }

        // Générer un nom avec une signification
        public static string GenerateWithMeaning(string meaning, string style = "Fantasy", int? seed = null)
        {
            // Table de correspondance simplifiée entre lettres et significations
            Dictionary<char, string[]> meaningMap = new Dictionary<char, string[]>
            {
                ['a'] = new[] { "strength", "power", "courage" },
                ['b'] = new[] { "beauty", "grace", "elegance" },
                ['c'] = new[] { "wisdom", "knowledge", "intelligence" },
                ['d'] = new[] { "earth", "ground", "stability" },
                ['e'] = new[] { "air", "wind", "freedom" },
                ['f'] = new[] { "fire", "passion", "energy" },
                ['g'] = new[] { "growth", "nature", "life" },
                ['h'] = new[] { "healing", "health", "restoration" },
                ['i'] = new[] { "ice", "cold", "calm" },
                ['j'] = new[] { "justice", "fairness", "balance" },
                ['k'] = new[] { "kinship", "family", "community" },
                ['l'] = new[] { "light", "illumination", "truth" },
                ['m'] = new[] { "magic", "mystery", "wonder" },
                ['n'] = new[] { "night", "darkness", "shadow" },
                ['o'] = new[] { "order", "structure", "organization" },
                ['p'] = new[] { "protection", "safety", "security" },
                ['q'] = new[] { "quest", "journey", "adventure" },
                ['r'] = new[] { "royalty", "nobility", "leadership" },
                ['s'] = new[] { "spirit", "soul", "essence" },
                ['t'] = new[] { "time", "eternity", "persistence" },
                ['u'] = new[] { "unity", "harmony", "peace" },
                ['v'] = new[] { "victory", "triumph", "success" },
                ['w'] = new[] { "water", "flow", "adaptability" },
                ['x'] = new[] { "unknown", "mystery", "enigma" },
                ['y'] = new[] { "youth", "vitality", "energy" },
                ['z'] = new[] { "zeal", "passion", "intensity" }
            };

            Random rng = seed.HasValue ? new Random(seed.Value) : new Random();

            // Trouver les lettres associées au sens
            List<char> relevantLetters = new List<char>();
            meaning = meaning.ToLower();

            foreach (var pair in meaningMap)
            {
                if (pair.Value.Any(m => meaning.Contains(m)))
                {
                    relevantLetters.Add(pair.Key);
                }
            }

            // Si aucune lettre pertinente n'est trouvée, utiliser des lettres aléatoires
            if (relevantLetters.Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    relevantLetters.Add((char)('a' + rng.Next(26)));
                }
            }

            // Créer un modèle personnalisé qui inclut ces lettres
            StringBuilder patternBuilder = new StringBuilder();

            // Commencer par une lettre significative
            patternBuilder.Append(relevantLetters[rng.Next(relevantLetters.Count)]);

            // Ajouter un modèle de base selon le style
            if (Structures.TryGetValue(style, out PhoneticStructure structure))
            {
                string basePattern = structure.Patterns[rng.Next(structure.Patterns.Length)];
                patternBuilder.Append(basePattern);

                // Insérer au moins une autre lettre significative
                int insertPos = rng.Next(1, patternBuilder.Length);
                patternBuilder.Insert(insertPos, relevantLetters[rng.Next(relevantLetters.Count)]);
            }

            // Générer le nom
            string name = Generate(style, 4, 12, seed);

            // S'assurer qu'au moins une lettre significative est incluse
            if (!relevantLetters.Any(l => name.ToLower().Contains(l)))
            {
                int pos = rng.Next(1, name.Length);
                name = name.Insert(pos, relevantLetters[rng.Next(relevantLetters.Count)].ToString());
            }

            return name;
        }

        // Structure phonétique pour un style de noms
        private class PhoneticStructure
        {
            public string[] Patterns;
            public string Consonants;
            public string Vowels;
            public Dictionary<char, string> SpecialConsonants;
            public string[] ForbiddenPatterns;
            public Dictionary<string, string> Substitutions;
            public Dictionary<string, string[]> Mutations;
        }
    }
}