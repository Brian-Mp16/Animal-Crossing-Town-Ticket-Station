﻿using Animal_Crossing_Town_Ticket_Station;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Crossing_Town_Ticket_Station
{
    internal class FishData
    {
        private static readonly FishInfo[] FishInfoArray =
        {
           new FishInfo("Catch a Crucian Carp", 1, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 200, Properties.Resources.FB_Crucian_Carp_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Brook Trout", 2, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 250, Properties.Resources.FB_Brook_Trout_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Carp", 3, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 200, Properties.Resources.FB_Carp_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Koi", 4, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 500, Properties.Resources.FB_Koi_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Barbel Steed", 5, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 200, Properties.Resources.FB_Barbel_Steed_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Dace", 6, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 200, Properties.Resources.FB_Dace_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Catfish", 7, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 250, Properties.Resources.FB_Catfish_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Giant Catfish", 8, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 400, Properties.Resources.FB_Giant_Catfish_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Pale Chub", 9, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, null, new int?[1,2]{ {0,0} }, 250, Properties.Resources.FB_Pale_Chub_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Bitterling", 10, new int?[12,2]{ {31,1}, {31,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 300, Properties.Resources.FB_Bitterling_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Loach", 11, new int?[12,2]{ {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 250, Properties.Resources.FB_Loach_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Bluegill", 12, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, null, new int?[1,2]{ {0,0} }, 250, Properties.Resources.FB_Bluegill_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Small Bass", 13, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 200, Properties.Resources.FB_Small_Bass_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Bass", 14, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 200, Properties.Resources.FB_Bass_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Large Bass", 15, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 350, Properties.Resources.FB_Large_Bass_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Giant Snakehead", 16, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, null, new int?[1,2]{ {0,0} }, 400, Properties.Resources.FB_Giant_Snakehead_PG_Field_Sprite_Upscaled),
new FishInfo("Catch an Eel", 17, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 350, Properties.Resources.FB_Eel_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Freshwater Goby", 18, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 300, Properties.Resources.FB_Freshwater_Goby_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Pond Smelt", 19, new int?[12,2]{ {31,1}, {31,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 200, Properties.Resources.FB_Pond_Smelt_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Sweetfish", 20, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 250, Properties.Resources.FB_Sweetfish_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Cherry Salmon", 21, new int?[12,2]{ {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {0,0} }, new int?[24,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0} }, null, new int?[1,2]{ {0,0} }, 300, Properties.Resources.FB_Cherry_Salmon_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Rainbow Trout", 22, new int?[12,2]{ {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {0,0} }, new int?[24,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0} }, null, new int?[1,2]{ {0,0} }, 300, Properties.Resources.FB_Rainbow_Trout_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Large Char", 23, new int?[12,2]{ {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {0,0} }, new int?[24,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0} }, null, new int?[1,2]{ {0,0} }, 400, Properties.Resources.FB_Large_Char_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Stringfish", 24, new int?[12,2]{ {31,1}, {31,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 500, Properties.Resources.FB_Stringfish_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Salmon", 25, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,1}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 250, Properties.Resources.FB_Salmon_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Goldfish", 26, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 350, Properties.Resources.FB_Goldfish_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Popeyed Goldfish", 27, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, null, new int?[1,2]{ {0,0} }, 350, Properties.Resources.FB_Popeyed_Goldfish_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Guppy", 28, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0} }, new int?[24,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, null, new int?[1,2]{ {0,0} }, 450, Properties.Resources.FB_Guppy_PG_Field_Sprite_Upscaled),
new FishInfo("Catch an Angelfish", 29, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 500, Properties.Resources.FB_Angelfish_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Piranha", 30, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 500, Properties.Resources.FB_Piranha_PG_Field_Sprite_Upscaled),
new FishInfo("Catch an Arowana", 31, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0} }, null, new int?[1,2]{ {0,0} }, 500, Properties.Resources.FB_Arowana_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Coelacanth", 32, new int?[12,2]{ {0,0}, {25,2}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {1,1} }, 400, Properties.Resources.FB_Coelacanth_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Crawfish", 33, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {15,1}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 200, Properties.Resources.FB_Crawfish_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Frog", 34, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 200, Properties.Resources.FB_Frog_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Killifish", 35, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 350, Properties.Resources.FB_Killifish_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Jellyfish", 36, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {15,2}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 250, Properties.Resources.FB_Jellyfish_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Sea Bass", 37, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {15,1}, {15,2}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 200, Properties.Resources.FB_Sea_Bass_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Red Snapper", 38, new int?[12,2]{ {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 300, Properties.Resources.FB_Red_Snapper_PG_Field_Sprite_Upscaled),
new FishInfo("Catch a Barred Knifejaw", 39, new int?[12,2]{ {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {31,1}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 400, Properties.Resources.FB_Barred_Knifejaw_PG_Field_Sprite_Upscaled),
new FishInfo("Catch an Arapaima", 40, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,1}, {31,1}, {31,1}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, null, new int?[1,2]{ {0,0} }, 500, Properties.Resources.FB_Arapaima_PG_Field_Sprite_Upscaled),

};

        public static int GetFishInfoArrayLength()
        {
            return FishInfoArray.Length;
        }

        public static Tuple<FishInfo, int> GetFishCheckByName(string name)
        {
            for (int i = 0; i < GetFishInfoArrayLength(); i++)
                if (FishInfoArray[i].Name.Equals(name))
                    return new Tuple<FishInfo, int>(FishInfoArray[i], 1);

            return new Tuple<FishInfo, int>(null, 0);
        }

        public static Tuple<FishInfo, int> GetFishCheckByIndex(int index)
        {
            for (int i = 0; i < GetFishInfoArrayLength(); i++)
                if (FishInfoArray[i].Index.Equals(index))
                    return new Tuple<FishInfo, int>(FishInfoArray[i], 1);

            return new Tuple<FishInfo, int>(null, 0);
        }
    }
}
