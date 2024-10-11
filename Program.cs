using EXAM2_Opgave4_TCPApp_TEK3_2024;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text.Json;


class Program
{
    static void Main()
    {
        // Print sth. to identify app
        Console.WriteLine("TCP Server:");

        // Instantiate new object of TcpListener class, with port 8
        TcpListener listener = new TcpListener(IPAddress.Any, 8);

        // Start listening for connections
        listener.Start();
        Console.WriteLine("Server is listening on port 8...");

        // Main server loop - this will keep the server running indefinitely
        while (true)
        {
            // Accept a new client connection
            TcpClient socket = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            // Handle the client in a separate task/thread to allow multiple connections simultaneously
            Task.Run(() => HandleClient(socket));
        }
    }

    // Handle client connections in a separate method
    static void HandleClient(TcpClient socket)
    {
        try
        {
            // Read and write connections (streams)
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            string message;

            // Keep reading messages until the client disconnects
            while ((message = reader.ReadLine()) != null)
            {

                // Randomizer
                if (message.Contains("Random", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Great. Let's generate random numbers. Input numbers: ");
                    writer.Flush();

                    // Wait for the client to send two numbers
                    string numbers = reader.ReadLine();

                    // Split numbers into two elements in a list
                    string[] splitNumbers = numbers.Split(' ');
                    int num1 = Int32.Parse(splitNumbers[0]);
                    int num2 = Int32.Parse(splitNumbers[1]);

                    // Generate random number        
                    Random rand = new Random();
                    int randomNumber = rand.Next(Math.Min(num1, num2), Math.Max(num1, num2) + 1);

                    Console.WriteLine("Result: "+randomNumber);

                    // Put variables into Message object
                    Message messageRandom = new Message(message, num1, num2);

                    // Serialize into JSON
                    string messageRandomJson = JsonSerializer.Serialize(messageRandom);
                    Console.WriteLine(messageRandomJson);

                    writer.WriteLine(messageRandomJson); // Send JSON back to client
                    writer.Flush();
                    continue;  // Continue reading new messages after handling this command
                }

                // Add
                if (message.Contains("Add", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Great. Let's add two numbers. Input numbers: ");
                    writer.Flush();

                    // Wait for the client to send two numbers
                    string numbers = reader.ReadLine();

                    // Split numbers into two elements in a list
                    string[] splitNumbers = numbers.Split(' ');
                    int num1 = Int32.Parse(splitNumbers[0]);
                    int num2 = Int32.Parse(splitNumbers[1]);

                    // Addition
                    int result = num1 + num2;

                    Console.WriteLine("Result: "+ result);

                    // Put variables into Message object
                    Message messageAdd = new Message(message, num1, num2);

                    // Serialize into JSON
                    string messageAddJson = JsonSerializer.Serialize(messageAdd);
                    Console.WriteLine(messageAddJson);

                    writer.WriteLine(messageAddJson); // Send JSON back to client
                    writer.Flush();
                    continue;  // Continue reading new messages after handling this command
                }

                // Subtract
                if (message.Contains("Subtract", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Great. Let's subtract two numbers. Input numbers: ");
                    writer.Flush();

                    // Wait for the client to send two numbers
                    string numbers = reader.ReadLine();

                    // Split numbers into two elements in a list
                    string[] splitNumbers = numbers.Split(' ');
                    int num1 = Int32.Parse(splitNumbers[0]);
                    int num2 = Int32.Parse(splitNumbers[1]);

                    // Subtraction
                    int result = num1 - num2;

                    Console.WriteLine("Result: "+result);

                    // Put variables into Message object
                    Message messageSubtract = new Message(message, num1, num2);

                    // Serialize into JSON
                    string messageSubtractJson = JsonSerializer.Serialize(messageSubtract);
                    Console.WriteLine(messageSubtractJson);

                    writer.WriteLine(messageSubtractJson); // Send JSON back to client
                    writer.Flush();
                    continue;  // Continue reading new messages after handling this command
                }

                // Stop command
                if (message.Equals("Stop", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Stop command received. Closing connection.");
                    writer.WriteLine("Goodbye!"); // Send a message back to the client
                    writer.Flush();
                    break; // Exit the loop to close the connection
                }

                // If no valid command is matched, throw an exception
                else
                {
                    throw new Exception("Unrecognized command. Please use 'Random', 'Add', 'Subtract', or 'Stop'.");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
        finally
        {
            // Clean up and close the connection
            socket.Close();
            Console.WriteLine("Client disconnected.");
        }
    }

}