﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regexer {
    class Program {
        static void Main( string[ ] args ) {
            string pattern = @"((a+)b|(b+)a)+", input = "aabbbaaaab";

            var rex = new Regex( pattern );
            Console.WriteLine( pattern );
            Console.WriteLine( rex.ToLISPyString( ) );

            foreach ( RegexMatch match in rex.Matches( input ) ) {
                Console.WriteLine( match );

                foreach ( RegexGroup group in match.Groups ) {
                    Console.WriteLine( "\tGroup {0}: {1} - {2} \"{3}\"",
                        group.Index, group.Start, group.End, group );
                }
            }

            Console.ReadKey( true );
        }
    }
}
