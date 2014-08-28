﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regexer {


    /** The GroupToken represents a sequence of patterns which must appear
     *  in the specified order in the input string; groups induced by round
     *  brackets are also represented by GroupTokens.
     *  
     *  For example,
     *   - The pattern ab+c is compiled to a GroupToken containing three
     *     distinct patterns: a, b+ and c.
     *     
     *   - The pattern (\w+)(\d+) is compiled to a GroupToken containing the
     *     two patterns \w+ and \d+.
     */
    public class GroupToken : Token {
        public List<Token> Content { get; set; }    ///< The content of this group. Order is important.

        /** Create a new GroupToken.
         */
        public GroupToken( )
            : base( TokenType.Group, "()" ) {

            Content = new List<Token>( );
        }


        public override bool Matches( string input, ref int cursor ) {
            return matchesFrom( 0, input, ref cursor );
        }


        public override bool DoBacktrack( string input, ref int cursor ) {
            int i = Content.Count;

            if ( findBacktrackToken( input, ref cursor, ref i ) )
                return matchesFrom( ++i, input, ref cursor );
            else return false;
        }


        /** Checks wheter the input, starting at the cursor, matches all the
         *  tokens, starting at the given point, in the correct sequence.
         *  
         *  If the matching succeeds, the cursor will be moved after the
         *  end of the match; on the contrary, if the matching fails, its
         *  value will not be modified.
         * 
         *  \param start The token from where start matching (it will be included)
         *  \param input The input we are trying to match
         *  \param cursor The current position in the input. If the input matches
         *  the cursor will be moved after the end of the match, it won't be
         *  affected otherwise.
         *  \return True if the input matches this token. In this case the cursor
         *  will also be moved after the end of the match.
         */
        private bool matchesFrom( int start, string input, ref int cursor ) {
            int cursor_start = cursor;

            for ( ; start < Content.Count; start++ ) {
                if ( !Content[ start ].Matches( input, ref cursor ) ) {
                    if ( !findBacktrackToken( input, ref cursor, ref start ) ) {
                        cursor = cursor_start;
                        return false;
                    }
                }
            }

            return true;
        }

        /** Find the first token which can backtrack, starting at the given point and going
         *  backwards. Backtracking will be performed if possible, moving the cursor to
         *  the correct position and saving the token which backtracked.
         * 
         *  \param input The input that we are trying to match
         *  \param cursor The current position in the input. It will be moved to the
         *  correct position if backtracking is possible.
         *  \param token Where to start. Note that this token will not be asked for
         *  backtracking; this method starts from the previous one.
         *  \returns True if it was possible to backtrack. In this case, both cursor and
         *  token will be modified to reflect the changes.
         */
        private bool findBacktrackToken( string input, ref int cursor, ref int token ) {
            int start = cursor;

            while ( --token >= 0 ) {
                if ( Content[ token ].DoBacktrack( input, ref cursor ) )
                    return true;
            }

            cursor = start;
            return false;
        }


        /** String representation of this group token; the tokens contained in
         *  this group are included
         *  
         *  \returns The string representation of this token.
         */
        protected override string printContent( ) {
            var sb = new StringBuilder( );
            foreach ( Token t in Content )
                sb.AppendLine( t.ToString( ) );
            return sb.ToString( );
        }
    }
}