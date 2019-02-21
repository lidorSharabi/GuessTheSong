using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheSongServer.DB
{
    public static class QuestionQueries
    {
        public static string artist_genre_years_Query = @"SELECT lyrics.song, lyrics.artist, lyrics.lyrics FROM guessthesong.lyrics
WHERE lyrics.artist = '{0}'
AND lyrics.genre = '{1}'
AND (lyrics.year >= '{2}' AND lyrics.year < '{3}')
LIMIT 20";

        public static string artist_diffGenre_Query = @"SELECT lyrics.song, lyrics.artist, lyrics.lyrics
FROM guessthesong.lyrics
WHERE lyrics.artist = '{0}'
AND lyrics.genre != '{1}'
LIMIT 20";

        public static string artist_genre_widerYears_Query = @"SELECT lyrics.song, lyrics.artist, lyrics.lyrics FROM guessthesong.lyrics
WHERE lyrics.artist = '{0}'
AND lyrics.genre = '{1}'
AND (lyrics.year >= '{2}-10' AND lyrics.year < '{3}+10')
LIMIT 20";

        public static string mostPopularArtist_genre_years = @"SELECT song, artist, lyrics FROM
(SELECT lyrics.song, lyrics.artist, lyrics.lyrics, artists.familiarity, artists.popularity, MAX(familiarity), MAX(popularity)
FROM guessthesong.artists
INNER JOIN guessthesong.lyrics
ON artists.id = lyrics.artist
WHERE lyrics.genre = '{0}'
AND (lyrics.year >= '{1}' AND lyrics.year < '{2}')
GROUP BY artists.id
HAVING artists.familiarity >= MAX(familiarity)-2 OR artists.popularity >= MAX(popularity)-2) as t
LIMIT 20";

        public static string diffNonPopularArtist_genre_years = @"SELECT lyrics.song, lyrics.artist, lyrics.lyrics
FROM guessthesong.artists 
INNER JOIN guessthesong.lyrics
ON artists.id = lyrics.artist
WHERE lyrics.artist != '{0}'
AND lyrics.genre = '{1}'
AND (lyrics.year >= '{2}' AND lyrics.year < '{3}')
ORDER BY artists.familiarity, artists.popularity ASC
LIMIT 20";

        public static string nonPopularArtist_genre_years = @"SELECT lyrics.song, lyrics.artist, lyrics.lyrics
FROM guessthesong.artists 
INNER JOIN guessthesong.lyrics
ON artists.id = lyrics.artist
WHERE lyrics.genre = '{0}'
AND (lyrics.year >= '{1}' AND lyrics.year < '{2}')
ORDER BY artists.familiarity, artists.popularity ASC
LIMIT 20";

        public static string popularArtist_genre_diffYears = @"SELECT lyrics.song, lyrics.artist, lyrics.lyrics
FROM guessthesong.artists 
INNER JOIN guessthesong.lyrics
ON artists.id = lyrics.artist
WHERE lyrics.genre = '{0}'
AND ((lyrics.year >= '{1}'-10 AND lyrics.year < '{2}'-10)
OR (lyrics.year >= '{1}'+10 AND lyrics.year < '{2}'+10))
ORDER BY artists.familiarity, artists.popularity DESC
LIMIT 20";


    }
}
