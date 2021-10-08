using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly List<Color> _colors;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.

        }


        public SearchResults Search(SearchOptions options)
        {
            var resShirts = new List<Shirt>();
            var resSizeCounts = new List<SizeCount>()
            {
                new SizeCount {Size = Size.Small},
                new SizeCount {Size = Size.Medium},
                new SizeCount {Size = Size.Large}
            };
            var resColorCounts = new List<ColorCount>()
            {
                new ColorCount {Color = Color.Black},
                new ColorCount {Color = Color.Blue},
                new ColorCount {Color = Color.Red},
                new ColorCount {Color = Color.White},
                new ColorCount {Color = Color.Yellow},
            };

            //var dictSizeCounts = new Dictionary<Guid, int>()
            //{
            //    {Size.Small.Id, 0},
            //    {Size.Medium.Id, 0},
            //    {Size.Large.Id, 0}
            //};


            if (!options.Sizes.Any())
            {
                var tt = from color in options.Colors
                         from shirt in _shirts
                         where shirt.Color.Id == color.Id
                         select new
                         {
                             shirt,

                         };

                foreach (var color in options.Colors)
                {
                    var count = _shirts.Count(s => s.Color.Id == color.Id);
                    resColorCounts.Where(x => x.Color.Name == color.Name).Select(x =>
                    {
                        x.Count = count;
                        return x;
                    }).ToList();
                }

                resShirts = tt.Select(x => x.shirt).ToList();



                return new SearchResults
                {
                    Shirts = resShirts,
                    SizeCounts = resSizeCounts,
                    ColorCounts = resColorCounts
                };
            }
            else
            {
                var tt = from color in options.Colors
                         from size in options.Sizes
                         from shirt in _shirts
                         where shirt.Color.Id == color.Id && shirt.Size.Id == size.Id
                         select new
                         {
                             shirt,

                         };


                foreach (var size in options.Sizes)
                {
                    var count = tt.Count(s => s.shirt.Size.Id == size.Id);

                    resSizeCounts.Where(x => x.Size.Name == size.Name).Select(x =>
                    {
                        x.Count = count;
                        return x;
                    }).ToList();

                    //dictSizeCounts[size.Id] = count;
                }

                foreach (var color in options.Colors)
                {
                    var count = tt.Count(s => s.shirt.Color.Id == color.Id);
                    resColorCounts.Where(x => x.Color.Name == color.Name).Select(x =>
                    {
                        x.Count = count;
                        return x;
                    }).ToList();
                }


                //foreach (var size in options.Sizes)
                //{
                //    var count = _shirts.Count(s => s.Size.Id == size.Id);

                //    resSizeCounts.Where(x => x.Size.Name == size.Name).Select(x =>
                //    {
                //        x.Count = count;
                //        return x;
                //    }).ToList();

                //    //dictSizeCounts[size.Id] = count;
                //}

                //foreach (var color in options.Colors)
                //{
                //    var count = _shirts.Count(s => s.Color.Id == color.Id);
                //    resColorCounts.Where(x => x.Color.Name == color.Name).Select(x =>
                //    {
                //        x.Count = count;
                //        return x;
                //    }).ToList();
                //}



                resShirts = tt.Select(x => x.shirt).ToList();


                return new SearchResults
                {
                    Shirts = resShirts,
                    SizeCounts = resSizeCounts,
                    ColorCounts = resColorCounts
                };
            }
        }
    }
}