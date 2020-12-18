using MerchantRPG.Models.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameplayLoopCombat1.classes
{
    public class Equipment : DSI
    {
        public string Description { get; set; }
        public Equipment(int dex, int str, int intel, string description)
        {
            Dexterity = dex;
            Strength = str;
            Intelligence = intel;
            Description = description;
        }

        public Equipment(InventoryItem ii)
        {
            this.Description = ii.Name;
            this.Strength = ii.Stat == Stats.Strength ? Strength + ii.StatModifier : this.Strength;
            this.Dexterity = ii.Stat == Stats.Dexterity ? Dexterity + ii.StatModifier : this.Dexterity;
            this.Intelligence = ii.Stat == Stats.Intelligencge ? ii.StatModifier : this.Intelligence;
        }

        public Equipment(Character owner)
        {
            Random rand = new Random();

            Dexterity = Convert.ToInt32(owner.Dexterity * (rand.Next(0, 20) / 100));
            Strength = Convert.ToInt32(owner.Strength * (rand.Next(0, 20) / 100));
            Intelligence = Convert.ToInt32(owner.Intelligence * (rand.Next(0, 20) / 100));

            if(AbilityLevel == 0)
            {
                int pityPoint = rand.Next(0, 3);
                switch(pityPoint)
                {
                    case 0:
                        Dexterity++;
                        break;
                    case 1:
                        Strength++;
                        break;
                    case 2:
                        Intelligence++;
                        break;
                }
            }

            int attrRand = rand.Next(0, 100);
            if (attrRand < 11)
            {
                int negDexRand = rand.Next(0, 100);
                if (negDexRand > 50)
                    Strength += Dexterity / 2;
                else
                    Intelligence += Dexterity / 2;

                Dexterity *= -1;
            }
            else if (attrRand < 22)
            {
                int negStrRand = rand.Next(0, 100);
                if (negStrRand > 50)
                    Dexterity += Strength / 2;
                else
                    Intelligence += Strength / 2;

                Strength *= -1;
            }
            else if (attrRand < 33)
            {
                int negIntRand = rand.Next(0, 100);
                if (negIntRand > 50)
                    Dexterity += Intelligence / 2;
                else
                    Strength += Intelligence / 2;

                Intelligence *= -1;
            }

            
            int randAdj = rand.Next(0, Enum.GetValues(typeof(Adjectives)).Length);
            int randThing = rand.Next(0, Enum.GetValues(typeof(Things)).Length);
            Description = (Adjectives)randAdj + " " + (Things)randThing + " of " + PrimaryStat;

            DistributeEquipment(owner);
        }

        public void DistributeEquipment(Character owner)
        {
            Console.WriteLine("You've found: " + Description + " on " + owner.Name + " corpse.");
            Console.WriteLine("Dex: " + Dexterity + " - Str: " + Strength + " - Int: " + Intelligence);
            Console.WriteLine("Do you wish to equip it? y/n");
            string doEquip = Console.ReadLine();
            if(doEquip.ToLower() == "y")
            {
                Console.WriteLine("Equip to whom?");
                int characterIndex = 0;
                Character[] party = Party.Members.ToArray();
                foreach (Character player in party)
                {
                    Console.WriteLine( characterIndex + ". " + player.Name);
                    characterIndex++;
                }
                int choice = -1;
                while (choice > characterIndex || choice < 0)
                {
                    Int32.TryParse(Console.ReadLine(), out choice);
                }
                party[choice].AddEquipment(this);
                Console.WriteLine("You have equipped " + Description + " to " + party[choice].Name);
                Console.WriteLine("---continue---");
                Console.ReadLine();
            }
        }

        public enum Adjectives
        {
            abandoned, able, absolute, adorable, adventurous, academic, acceptable, acclaimed, accomplished, accurate, aching, acidic, acrobatic, active, actual, adept, admirable, admired, adolescent, adored, advanced, afraid, affectionate, aged, aggravating, aggressive, agile, agitated, agonizing, agreeable, ajar, alarmed, alarming, alert, alienated, alive, all, altruistic, amazing, ambitious, ample, amused, amusing, anchored, ancient, angelic, angry, anguished, animated, annual, another, antique, anxious, any, apprehensive, appropriate, apt, arctic, arid, aromatic, artistic, ashamed, assured, astonishing, athletic, attached, attentive, attractive, austere, authentic, authorized, automatic, avaricious, average, aware, awesome, awful, awkward, babyish, bad, back, baggy, bare, barren, basic, beautiful, belated, beloved, beneficial, better, best, bewitched, big, bighearted, biodegradable, bitesized, bitter, black, blackandwhite, bland, blank, blaring, bleak, blind, blissful, blond, blue, blushing, bogus, boiling, bold, bony, boring, bossy, both, bouncy, bountiful, bowed, brave, breakable, brief, bright, brilliant, brisk, broken, bronze, brown, bruised, bubbly, bulky, bumpy, buoyant, burdensome, burly, bustling, busy, buttery, buzzing, calculating, calm, candid, canine, capital, carefree, careful, careless, caring, cautious, cavernous, celebrated, charming, cheap, cheerful, cheery, chief, chilly, chubby, circular, classic, clean, clear, clearcut, clever, close, closed, cloudy, clueless, clumsy, cluttered, coarse, cold, colorful, colorless, colossal, comfortable, common, compassionate, competent, complete, complex, complicated, composed, concerned, concrete, confused, conscious, considerate, constant, content, conventional, cooked, cool, cooperative, coordinated, corny, corrupt, costly, courageous, courteous, crafty, crazy, creamy, creative, creepy, criminal, crisp, critical, crooked, crowded, cruel, crushing, cuddly, cultivated, cultured, cumbersome, curly, curvy, cute, cylindrical, damaged, damp, dangerous, dapper, daring, darling, dark, dazzling, dead, deadly, deafening, dear, dearest, decent, decisive, deep, defenseless, defensive, defiant, deficient, definite, definitive, delayed, delectable, delicious, delightful, delirious, demanding, dense, dental, dependable, dependent, descriptive, deserted, detailed, determined, devoted, different, difficult, digital, diligent, dim, dimpled, dimwitted, direct, disastrous, discrete, disfigured, disgusting, disloyal, dismal, distant, downright, dreary, dirty, disguised, dishonest, distinct, distorted, dizzy, dopey, doting, drab, drafty, dramatic, droopy, dry, dual, dull, dutiful, each, eager, earnest, early, easy, easygoing, ecstatic, edible, educated, elaborate, elastic, elated, elderly, electric, elegant, elementary, elliptical, embarrassed, embellished, eminent, emotional, empty, enchanted, enchanting, energetic, enlightened, enormous, enraged, entire, envious, equal, equatorial, essential, esteemed, ethical, euphoric, even, evergreen, everlasting, every, evil, exalted, excellent, exemplary, exhausted, excitable, excited, exciting, exotic, expensive, experienced, expert, extraneous, extroverted, extralarge, extrasmall, fabulous, failing, faint, fair, faithful, fake, familiar, famous, fancy, fantastic, far, faraway, farflung, faroff, fast, fat, fatal, fatherly, favorable, favorite, fearful, fearless, feisty, feline, female, feminine, few, fickle, filthy, fine, finished, firm, first, firsthand, fitting, flaky, flamboyant, flashy, flat, flawed, flawless, flickering, flimsy, flippant, flowery, fluffy, fluid, flustered, focused, fond, foolhardy, foolish, forceful, forked, formal, forsaken, forthright, fortunate, fragrant, frail, frank, frayed, free, French, fresh, frequent, friendly, frightened, frightening, frigid, frilly, frizzy, frivolous, front, frosty, frozen, frugal, fruitful, full, fumbling, functional, funny, fussy, fuzzy, gargantuan, gaseous, general, generous, gentle, genuine, giant, giddy, gigantic, gifted, giving, glamorous, glaring, glass, gleaming, gleeful, glistening, glittering, gloomy, glorious, glossy, glum, golden, good, natured, gorgeous, graceful, gracious, grand, grandiose, granular, grateful, grave, gray, great, greedy, green, gregarious, grim, grimy, gripping, grizzled, gross, grotesque, grouchy, grounded, growing, growling, grown, grubby, gruesome, grumpy, guilty, gullible, gummy, hairy, half, handmade, handsome, handy, happy, go, lucky, hard, to,find, harmful, harmless, harmonious, harsh, hasty, hateful, haunting, healthy, heartfelt, hearty, heavenly, heavy, hefty, helpful, helpless, hidden, hideous, high, level, hilarious, hoarse, hollow, homely, honest, honorable, honored, hopeful, horrible, hospitable, hot, huge, humble, humiliating, humming, humongous, hungry, hurtful, husky, icky, icy, ideal, idealistic, identical, idle, idiotic, idolized, ignorant, ill, illegal, fated, informed, illiterate, illustrious, imaginary, imaginative, immaculate, immaterial, immediate, immense, impassioned, impeccable, impartial, imperfect, imperturbable, impish, impolite, important, impossible, impractical, impressionable, impressive, improbable, impure, inborn, incomparable, incompatible, incomplete, inconsequential, incredible, indelible, inexperienced, indolent, infamous, infantile, infatuated, inferior, infinite, informal, innocent, insecure, insidious, insignificant, insistent, instructive, insubstantial, intelligent, intent, intentional, interesting, international, intrepid, ironclad, irresponsible, irritating, itchy, jaded, jagged, jampacked, jaunty, jealous, jittery, joint, jolly, jovial, joyful, joyous, jubilant, judicious, juicy, jumbo, junior, jumpy, juvenile, kaleidoscopic, keen, key, kind, kindhearted, kindly, klutzy, knobby, knotty, knowledgeable, knowing, known, kooky, kosher, lame, lanky, large, last, lasting, late, lavish, lawful, lazy, leading, lean, leafy, left, legal, legitimate, light, lighthearted, likable, likely, limited, limp, limping, linear, lined, liquid, little, live, lively, livid, loathsome, lone, lonely, loose, lopsided, lost, loud, lovable, lovely, loving, low, loyal, lumbering, luminous, lumpy, lustrous, luxurious, mad, madeup, magnificent, majestic, major, male, mammoth, married, marvelous, masculine, massive, mature, meager, mealy, mean, measly, meaty, medical, mediocre, medium, meek, mellow, melodic, memorable, menacing, merry, messy, metallic, mild, milky, mindless, miniature, minor, minty, miserable, miserly, misguided, misty, mixed, modern, modest, moist, monstrous, monthly, monumental, moral, mortified, motherly, motionless, mountainous, muddy, muffled, multicolored, mundane, murky, mushy, musty, muted, mysterious, naive, narrow, nasty, natural, naughty, nautical, near, neat, necessary, needy, negative, neglected, negligible, neighboring, nervous, next, nice, nifty, nimble, nippy, nocturnal, noisy, nonstop, normal, notable, noted, noteworthy, novel, noxious, numb, nutritious, nutty, obedient, obese, oblong, oily, obvious, occasional, odd, oddball, offbeat, offensive, official, old, oldfashioned, only, open, optimal, optimistic, opulent, orange, orderly, organic, ornate, ornery, ordinary, original, other, our, outlying, outgoing, outlandish, outrageous, outstanding, oval, overcooked, overdue, overjoyed, overlooked, palatable, pale, paltry, parallel, parched, partial, passionate, past, pastel, peaceful, peppery, perfect, perfumed, periodic, perky, personal, pertinent, pesky, pessimistic, petty, phony, physical, piercing, pink, pitiful, plain, plaintive, plastic, playful, pleasant, pleased, pleasing, plump, plush, polished, polite, political, pointed, pointless, poised, poor, popular, portly, posh, positive, possible, potable, powerful, powerless, practical, precious, present, prestigious, pretty, previous, pricey, prickly, primary, prime, pristine, prize, probable, productive, profitable, profuse, proper, proud, prudent, punctual, pungent, puny, pure, purple, pushy, putrid, puzzled, puzzling, quaint, qualified, quarrelsome, quarterly, queasy, querulous, questionable, quick, quickwitted, quiet, quintessential, quirky, quixotic, quizzical, radiant, ragged, rapid, rare, rash, raw, recent, reckless, rectangular, ready, real, realistic, reasonable, red, reflecting, regal, regular, reliable, relieved, remarkable, remorseful, remote, repentant, required, respectful, responsible, repulsive, revolving, rewarding, rich, rigid, right, ringed, ripe, roasted, robust, rosy, rotating, rotten, rough, round, rowdy, royal, rubbery, rundown, ruddy, rude, runny, rural, rusty, sad, safe, salty, same, sandy, sane, sarcastic, sardonic, satisfied, scaly, scarce, scared, scary, scented, scholarly, scientific, scornful, scratchy, scrawny, second, secondary, secondhand, secret, selfassured, selfreliant, selfish, sentimental, separate, serene, serious, serpentine, several, severe, shabby, shadowy, shady, shallow, shameful, shameless, sharp, shimmering, shiny, shocked, shocking, shoddy, showy, shrill, shy, sick, silent, silky, silly, silver, similar, simple, simplistic, sinful, single, sizzling, skeletal, skinny, sleepy, slight, slim, slimy, slippery, slow, slushy, small, smart, smoggy, smooth, smug, snappy, snarling, sneaky, sniveling, snoopy, sociable, soft, soggy, solid, somber, some, spherical, sophisticated, sore, sorrowful, soulful, soupy, sour, Spanish, sparkling, sparse, specific, spectacular, speedy, spicy, spiffy, spirited, spiteful, splendid, spotless, spotted, spry, square, squeaky, squiggly, stable, staid, stained, stale, standard, starchy, stark, starry, steep, sticky, stiff, stimulating, stingy, stormy, straight, strange, steel, strict, strident, striking, striped, strong, studious, stunning, stupendous, stupid, sturdy, stylish, subdued, submissive, substantial, subtle, suburban, sudden, sugary, sunny, super, superb, superficial, superior, supportive, surprised, suspicious, svelte, sweaty, sweet, sweltering, swift, sympathetic, tall, talkative, tame, tan, tangible, tart, tasty, tattered, taut, tedious, teeming, tempting, tender, tense, tepid, terrible, terrific, testy, thankful, that, these, thick, thin, third, thirsty, thorough, thorny, those, thoughtful, threadbare, thrifty, thunderous, tidy, tight, timely, tinted, tiny, tired, torn, total, tough, traumatic, treasured, tremendous, tragic, trained, triangular, tricky, trifling, trim, trivial, troubled, trusting, trustworthy, trusty, truthful, tubby, turbulent, twin, ugly, ultimate, unacceptable, unaware, uncomfortable, uncommon, unconscious, understated, unequaled, uneven, unfinished, unfit, unfolded, unfortunate, unhappy, unhealthy, uniform, unimportant, unique, united, unkempt, unknown, unlawful, unlined, unlucky, unnatural, unpleasant, unrealistic, unripe, unruly, unselfish, unsightly, unsteady, unsung, untidy, untimely, untried, untrue, unused, unusual, unwelcome, unwieldy, unwilling, unwitting, unwritten, upbeat, upright, upset, urban, usable, used, useful, useless, utilized, utter, vacant, vague, vain, valid, valuable, vapid, variable, vast, velvety, venerated, vengeful, verifiable, vibrant, vicious, victorious, vigilant, vigorous, villainous, violet, violent, virtuous, visible, vital, vivacious, vivid, voluminous, wan, warlike, warm, warmhearted, warped, wary, wasteful, watchful, waterlogged, watery, wavy, wealthy, weak, weary, webbed, wee, weekly, weepy, weighty, weird, welcome, wet, which, whimsical, whirlwind, whispered, whole, whopping, wicked, wide, wiggly, wild, willing, wilted, winding, windy, winged, wiry, wise, witty, wobbly, woeful, wonderful, wooden, woozy, wordy, worldly, worn, worried, worrisome, worse, worst, worthless, worthwhile, worthy, wrathful, wretched, writhing, wrong, wry, yawning, yearly, yellow, yellowish, young, youthful, yummy, zany, zealous, zesty, zigzag
        
        }
        public enum Things
        {
            gloves, greeves, hat, boots, breastplate, jacket, shirt, pants, leggings, bow, staff, knife, sword, mace, crossbow, helmet, flail, wand, scimitar, machette, charm, necklace, ring, gem, robe, blouse, shoes, socks, underwear, chalice, cup, satchel, headgear, headdress, makeup
        }

    }

    public static class FindDrops
    {
        public static bool Search (Character[] participants)
        {
            //living player party ability level / killed things ability level
            var rand = new Random();
            double score = participants.Where(p => p.Health > 0 && p.Player).Sum(p => p.AbilityLevel) / participants.Where(p => p.Health <= 0 && !p.Player).Sum(p => p.AbilityLevel);
            int dropScore = rand.Next(0, 100);
            return (score * 100 > dropScore);
        }
    }
    
}
