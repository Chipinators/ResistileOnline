using System;

namespace ResistileConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
            Console.WriteLine("Test 1 complete! Enter to continue.");
            //Console.ReadLine();

            Test2();
            Console.WriteLine("Test 2 complete! Enter to continue.");
            //Console.ReadLine();

            Test3();
            Console.WriteLine("Test 3 complete! Enter to continue.");
            //Console.ReadLine();

            Test4();
            Console.WriteLine("Test 4 complete! Enter to continue.");
            //Console.ReadLine();

            Test5();
            Console.ReadLine();
        }

        private static void Test5()
        {
            GameControl myGame = new GameControl();

            GameNode aNode = new GameNodeTypeIResistor(3);

            myGame.AddGameNodeToBoard(aNode, new Coordinates(0, 1));
        }

        private static void Test1()
        {
            //No Parallel all resistor no Wire
            GameControl myGame = new GameControl();

            for (int i = 1; i < 6; i++)
            {
                GameNode element = new GameNodeTypeIResistor(1);
                element.Rotate();
                if (myGame.AddGameNodeToBoard(element, new Coordinates(i, 0)))
                    Console.WriteLine("Element to " + i + ", 0 added successfully..");
                else
                {
                    Console.WriteLine("problem adding an element");
                    return;
                }
            }

            GameNode element2 = new GameNodeTypeIIResistor(1);
            if (myGame.AddGameNodeToBoard(element2, new Coordinates(6, 0)))
                Console.WriteLine("Element to 6, 0 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            for (int i = 1; i < 6; i++)
            {
                GameNode element = new GameNodeTypeIResistor(1);
                if (myGame.AddGameNodeToBoard(element, new Coordinates(6, i)))
                    Console.WriteLine("Element to 6, " + i + " added successfully..");
                else
                {
                    Console.WriteLine("problem adding an element");
                    return;
                }
            }
            
            Console.WriteLine("Total resistance is: " + myGame.CalculateTotalResistance().ToString("F2"));
            Console.WriteLine("It should be ~ 11");
        }
        private static void Test2()
        {
            //No resistor just wire
            GameControl myGame = new GameControl();

            for (int i = 1; i < 6; i++)
            {
                GameNode element = new GameNodeTypeIWire();
                element.Rotate();
                if (myGame.AddGameNodeToBoard(element, new Coordinates(i, 0)))
                    Console.WriteLine("Element to " + i + ", 0 added successfully..");
                else
                {
                    Console.WriteLine("problem adding an element");
                    return;
                }
            }

            GameNode resistor2 = new GameNodeTypeIIWire();
            if (myGame.AddGameNodeToBoard(resistor2, new Coordinates(6, 0)))
                Console.WriteLine("Element to 6, 0 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            for (int i = 1; i < 6; i++)
            {
                GameNode element = new GameNodeTypeIWire();
                if (myGame.AddGameNodeToBoard(element, new Coordinates(6, i)))
                    Console.WriteLine("Element to 6, " + i + " added successfully..");
                else
                {
                    Console.WriteLine("problem adding an element");
                    return;
                }
            }
            Console.WriteLine("Total resistance is: " + myGame.CalculateTotalResistance().ToString("F2"));
            Console.WriteLine("It should be ~ 0");
        }
        private static void Test3()
        {
            //No one parallel
            GameControl myGame = new GameControl();

            for (int i = 1; i < 5; i++)
            {
                GameNode element = new GameNodeTypeIResistor(1);
                element.Rotate();
                if (myGame.AddGameNodeToBoard(element, new Coordinates(i, 0)))
                    Console.WriteLine("Element to " + i + ", 0 added successfully..");
                else
                {
                    Console.WriteLine("problem adding an element");
                    return;
                }
            }
            GameNode elementT1 = new GameNodeTypeTWire();
            if (myGame.AddGameNodeToBoard(elementT1, new Coordinates(5, 0)))
                Console.WriteLine("Element T to 5, 0 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            GameNode element2 = new GameNodeTypeIIResistor(3);
            element2.Rotate();
            element2.Rotate();
            if (myGame.AddGameNodeToBoard(element2, new Coordinates(5, 1)))
                Console.WriteLine("Element to 5, 1 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            GameNode element3 = new GameNodeTypeIIResistor(6);
            if (myGame.AddGameNodeToBoard(element3, new Coordinates(6, 0)))
                Console.WriteLine("Element to 6, 0 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            GameNode elementT2 = new GameNodeTypeTWire();
            elementT2.Rotate();
            if (myGame.AddGameNodeToBoard(elementT2, new Coordinates(6, 1)))
                Console.WriteLine("Element T to 6, 1 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            for (int i = 2; i < 6; i++)
            {
                GameNode element = new GameNodeTypeIResistor(1);
                if (myGame.AddGameNodeToBoard(element, new Coordinates(6, i)))
                    Console.WriteLine("Element to 6, " + i + " added successfully..");
                else
                {
                    Console.WriteLine("problem adding an element");
                    return;
                }
            }
            Console.WriteLine("Total resistance is: " + myGame.CalculateTotalResistance().ToString("F2"));
            Console.WriteLine("It should be ~ 10");
        }

        private static void Test4()
        {
            //No one parallel
            GameControl myGame = new GameControl();

            for (int i = 1; i < 3; i++)
            {
                GameNode element = new GameNodeTypeIResistor(1);
                element.Rotate();
                if (myGame.AddGameNodeToBoard(element, new Coordinates(i, 0)))
                    Console.WriteLine("Element to " + i + ", 0 added successfully..");
                else
                {
                    Console.WriteLine("problem adding an element");
                    return;
                }
            }
            GameNode elementT0 = new GameNodeTypeTWire();
            if (myGame.AddGameNodeToBoard(elementT0, new Coordinates(3, 0)))
                Console.WriteLine("Element T to 5, 0 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            GameNode element1 = new GameNodeTypeIResistor(1);
            element1.Rotate();
            if (myGame.AddGameNodeToBoard(element1, new Coordinates(4, 0)))
                Console.WriteLine("Element to " + 4 + ", 0 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            GameNode elementT1 = new GameNodeTypeTWire();
            if (myGame.AddGameNodeToBoard(elementT1, new Coordinates(5, 0)))
                Console.WriteLine("Element T to 5, 0 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            GameNode element2 = new GameNodeTypeIIResistor(3);
            element2.Rotate();
            element2.Rotate();
            if (myGame.AddGameNodeToBoard(element2, new Coordinates(5, 1)))
                Console.WriteLine("Element to 5, 1 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            GameNode element3 = new GameNodeTypeIIResistor(6);
            if (myGame.AddGameNodeToBoard(element3, new Coordinates(6, 0)))
                Console.WriteLine("Element to 6, 0 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            GameNode elementT2 = new GameNodeTypeTWire();
            elementT2.Rotate();
            if (myGame.AddGameNodeToBoard(elementT2, new Coordinates(6, 1)))
                Console.WriteLine("Element T to 6, 1 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            GameNode element4 = new GameNodeTypeIResistor(1);
            if (myGame.AddGameNodeToBoard(element4, new Coordinates(6, 2)))
                Console.WriteLine("Element to " + 6 + ", 2 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            GameNode elementT3 = new GameNodeTypeTWire();
            elementT3.Rotate();
            if (myGame.AddGameNodeToBoard(elementT3, new Coordinates(6, 3)))
                Console.WriteLine("Element T to 6, 3 added successfully..");
            else
            {
                Console.WriteLine("problem adding an element");
                return;
            }

            GameNode element5 = new GameNodeTypeIIWire();
            element5.Rotate(); element5.Rotate();
            myGame.AddGameNodeToBoard(element5, new Coordinates(3, 1));

            GameNode element6 = new GameNodeTypeIIWire();
            myGame.AddGameNodeToBoard(element6, new Coordinates(4, 1));

            GameNode element7 = new GameNodeTypeIIResistor(4);
            element7.Rotate(); element7.Rotate();
            myGame.AddGameNodeToBoard(element7, new Coordinates(4, 2));

            GameNode element8 = new GameNodeTypeIIWire();
            myGame.AddGameNodeToBoard(element8, new Coordinates(5, 2));

            GameNode element9 = new GameNodeTypeIIWire();
            element9.Rotate(); element9.Rotate();
            myGame.AddGameNodeToBoard(element9, new Coordinates(5, 3));


            for (int i = 4; i < 6; i++)
            {
                GameNode element = new GameNodeTypeIResistor(1);
                if (myGame.AddGameNodeToBoard(element, new Coordinates(6, i)))
                    Console.WriteLine("Element to 6, " + i + " added successfully..");
                else
                {
                    Console.WriteLine("problem adding an element");
                    return;
                }
            }
            Console.WriteLine("Total resistance is: " + myGame.CalculateTotalResistance().ToString("F2"));
            Console.WriteLine("It should be ~ 6");


        }
    }
}
