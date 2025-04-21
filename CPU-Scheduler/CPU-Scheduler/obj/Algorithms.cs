using System;

class MyProcess{
    int id;
    int arrivalTime;
    int burstTime;
    int remainingTime;
    int waitingTime;
    int turnaroundTime;
    int completionTime;

    public MyProcess(int tempID, int tempArrivalTime, int tempBurstTime){
        id = tempID;
        arrivalTime = tempArrivalTime;
        burstTime = tempBurstTime;
        remainingTime = burstTime;
    }
}
static class Algorithm{
//Shortest time remaining first
    public static void strfAlgorithm(string userInput){
        Console.WriteLine("Enter a number of processes to run: ");
        int numberOfProcesses = Console.ReadLine();
        MyProcess[] processes = new Process[numberOfProcesses];
        
        //Loop to fill the array of processes
        for(int i=0;i<numberOfProcesses;i++){
            Console.WriteLine("Enter the arrival time for Process "+(i+1)+" ");
            int arrivalTime = Console.ReadLine();
            Console.WriteLine("Enter the burst time for Process "+(i+1)+" ");
            int burstTime = Console.ReadLine();
            processes[i] = new Process(i+1, arrivalTime, burstTime);
        }
        Array.Sort(processes, )
    }

}