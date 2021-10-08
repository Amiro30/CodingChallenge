using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
        }

        public SearchResults Search(SearchOptions options)
        {
            var result = new SearchResults
            {
                Shirts = new List<Shirt>(),
                SizeCounts = new List<SizeCount>(),
                ColorCounts = new List<ColorCount>(),
                IsSucceed = true
            };


            try
            {
                if (options != null)
                {
                    if (options.Sizes.Any() && options.Colors.Any())
                    {
                        var filteredShirts = from color in options.Colors
                                             from size in options.Sizes
                                             from shirt in _shirts
                                             where shirt.Color.Id == color.Id && shirt.Size.Id == size.Id
                                             select new
                                             {
                                                 shirt
                                             };

                        result.Shirts = filteredShirts.Select(x => x.shirt).ToList();

                        result.SizeCounts = FillSizeCounts(options.Sizes, result.Shirts);
                        result.ColorCounts = FillColorCounts(options.Colors, result.Shirts);

                        return result;
                    }
                    //case when only colors presented in search options
                    else if (!options.Sizes.Any())
                    {
                        var filteredShirts = from color in options.Colors
                                             from shirt in _shirts
                                             where shirt.Color.Id == color.Id
                                             select new
                                             {
                                                 shirt
                                             };

                        result.Shirts = filteredShirts.Select(x => x.shirt).ToList();
                        result.ColorCounts = FillColorCounts(options.Colors, result.Shirts);

                        return result;
                    }
                    //case when only sizes presented in search options
                    else if (!options.Colors.Any())
                    {
                        var filteredShirts = from size in options.Sizes
                                             from shirt in _shirts
                                             where shirt.Size.Id == size.Id
                                             select new
                                             {
                                                 shirt
                                             };

                        result.Shirts = filteredShirts.Select(x => x.shirt).ToList();
                        result.SizeCounts = FillSizeCounts(options.Sizes, result.Shirts);

                        return result;
                    }
                }

                result.IsSucceed = false;
                return result;
            }

            catch (Exception ex)
            {
                //_logger.LogDebug(ex.Message)

                result.IsSucceed = false;
                return result;
            }
            
        }

        private List<ColorCount> FillColorCounts(List<Color> colors, List<Shirt> filteredShirts)
        {
            var colorCounts = new List<ColorCount>
            {
                new ColorCount {Color = Color.Black},
                new ColorCount {Color = Color.Blue},
                new ColorCount {Color = Color.Red},
                new ColorCount {Color = Color.White},
                new ColorCount {Color = Color.Yellow}
            };

            foreach (var color in colors)
            {
                var count = filteredShirts.Count(s => s.Color.Id == color.Id);
                colorCounts.Where(x => x.Color.Name == color.Name).Select(x =>
                {
                    x.Count = count;
                    return x;
                }).ToList();
            }

            return colorCounts;
        }

        private List<SizeCount> FillSizeCounts(List<Size> sizes, List<Shirt> filteredShirts)
        {
            var sizeCounts = new List<SizeCount>
            {
                new SizeCount {Size = Size.Small},
                new SizeCount {Size = Size.Medium},
                new SizeCount {Size = Size.Large}
            };

            foreach (var size in sizes)
            {
                var count = filteredShirts.Count(s => s.Size.Id == size.Id);

                sizeCounts.Where(x => x.Size.Name == size.Name).Select(x =>
                {
                    x.Count = count;
                    return x;
                }).ToList();
            }

            return sizeCounts;
        }
    }
}