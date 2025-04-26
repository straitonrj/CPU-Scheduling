// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Diagnostics;
using System;

class MyProcess{
    public int id;
    public int arrivalTime;
    public int burstTime;
    public int remainingTime;
    public int waitingTime;
    public int turnaroundTime;
    public int completionTime;

    public MyProcess(int tempID, int tempArrivalTime, int tempBurstTime){
        id = tempID;
        arrivalTime = tempArrivalTime;
        burstTime = tempBurstTime;
        remainingTime = burstTime;
    }
}
static class Algorithm{
//Shortest time remaining first
    public static void strfAlgorithm(){
        Console.WriteLine("Enter a number of processes to run: ");
        int numberOfProcesses = Convert.ToInt32(Console.ReadLine());
        MyProcess[] processes = new MyProcess[numberOfProcesses];
        
        //Loop to fill the array of processes
        for(int i=0;i<numberOfProcesses;i++){
            Console.Write("Enter the arrival time for this Process: ");
            int arrivalTime=Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter the burst time for this Process : ");
            int burstTime = Convert.ToInt32(Console.ReadLine());

            processes[i] = new MyProcess(i+1, arrivalTime, burstTime);
        }
        var sorted = processes.OrderBy(MyProcess => MyProcess.arrivalTime).ToArray();
        //testing that array was sorted correctly
        /*Console.WriteLine("Now printing unsorted array");
        for(int i=0;i<processes.Length;i++){
            Console.WriteLine(processes[i].arrivalTime);
        }

        Console.WriteLine("Now printing sorted array");
        for(int i=0;i<sorted.Length;i++){
            Console.WriteLine(sorted[i].arrivalTime);
        }*/

        int currentTime =0;
        int completed =0;
        //loop to begin executing the algorithm
        while(completed < numberOfProcesses){
            int idx = -1;
            for(int i=0;i<numberOfProcesses;i++){
                if(processes[i].arrivalTime <= currentTime && processes[i].remainingTime > 0 && (idx == -1 || processes[i].remainingTime < processes[idx].remainingTime)){
                    idx = i;
                }
            }
            if(idx!=-1){
                processes[idx].remainingTime--;
                currentTime++;
                if (processes[idx].remainingTime==0){
                    processes[idx].completionTime = currentTime;
                    processes[idx].turnaroundTime = currentTime - processes[idx].arrivalTime;
                    processes[idx].waitingTime = processes[idx].turnaroundTime - processes[idx].burstTime;
                    completed++;
                }
            }
            else{
                currentTime++;
            }
        }

        double totalWT =0;
        double totalTAT = 0;
        foreach (MyProcess temp in processes){
        
            totalWT += temp.waitingTime;
            totalTAT += temp.turnaroundTime;
            Console.WriteLine("Process "+ temp.id+" CT: "+temp.completionTime+" WT: "+temp.waitingTime+" TAT: "+temp.turnaroundTime);
        }
        Console.WriteLine("Avg WT: "+ totalWT/numberOfProcesses + " Avg TAT: "+ totalTAT/numberOfProcesses+" Throughput "+(double)numberOfProcesses/currentTime);
    }

    //Multi-Level Feedback Queue
    public static void mlfqAlgorithm(){
        Queue first = new Queue();
        Queue second = new Queue();
        Queue third = new Queue();
        Queue fourth = new Queue();

        Queue[] queues = {first, second, third, fourth};
        Console.WriteLine("Enter a number of processes to run: ");
        int numberOfProcesses = Convert.ToInt32(Console.ReadLine());
        MyProcess[] processes = new MyProcess[numberOfProcesses];
        //when a process is complete it will be moved to a new queue to measure stuff at the end
        Queue completedProcesses = new Queue();

        int currentTime =0;
        
        //Loop to fill the array of processes
        for(int i=0;i<numberOfProcesses;i++){
            Console.Write("Enter the arrival time for this Process: ");
            int arrivalTime=Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter the burst time for this Process : ");
            int burstTime = Convert.ToInt32(Console.ReadLine());

            processes[i] = new MyProcess(i+1, arrivalTime, burstTime);
        }
        //new processes are assigned highest priority
        foreach(MyProcess temp in processes){
            queues[0].Enqueue(temp);
        }
        
        //go through each queue
        for(int i=0;i<queues.Length;i++){
            //check if queue is empty, if it isn't execute first process
            while(queues[i].Count > 0){
                MyProcess tempProcess = (MyProcess)queues[i].Dequeue();
                Console.WriteLine("Running process: "+tempProcess.id+" from queue: "+i+" with burst time: "+tempProcess.burstTime);
                int timeSlice = 4;
                //If process completes
                if(tempProcess.burstTime <= timeSlice){
                    currentTime += tempProcess.burstTime;
                    tempProcess.burstTime =0;
                    tempProcess.remainingTime =0;
                    //Update info and add to completed processes queue
                    tempProcess.completionTime = currentTime;
                    tempProcess.turnaroundTime = currentTime - tempProcess.arrivalTime;
                    tempProcess.waitingTime = tempProcess.turnaroundTime - tempProcess.arrivalTime;
                    completedProcesses.Enqueue(tempProcess);
                }
                else{
                    currentTime +=4;
                    tempProcess.burstTime = tempProcess.burstTime -timeSlice;
                    tempProcess.remainingTime = tempProcess.burstTime;
                    tempProcess.completionTime = currentTime;
                }

                //Move process to lower queue if not finished
                if(tempProcess.burstTime > 0){
                    if(i<3){
                        queues[i+1].Enqueue(tempProcess);
                    }
                }
                else{
                    Console.WriteLine("Process: "+tempProcess.id+" completed\n");
                }
                if(AllQueuesEmpty(queues)){
                    break;
                } 
            }
        }
        double totalWT =0;
        double totalTAT =0;
        //printing out measurable info
        foreach (MyProcess temp in completedProcesses){
            totalWT += temp.waitingTime;
            totalTAT += temp.turnaroundTime;
            Console.WriteLine("Process "+temp.id+" CT: "+temp.completionTime+" WT: "+temp.waitingTime+" TAT: "+temp.turnaroundTime);
        }
        Console.WriteLine("Avg WT: "+totalWT/numberOfProcesses+" Avg TAT: "+totalTAT/numberOfProcesses + " Throughput: "+(double)numberOfProcesses/currentTime);
    }
    //check to make sure all queues are empty
    public static bool AllQueuesEmpty(Queue[] queues){
        foreach(Queue temp in queues){
            if(temp.Count > 0){
                return false;
            }
        }
        return true;
    }
}
class Run{
    public static void Main(string[] args){
        int userInput=0;
        while(userInput !=3){
            Console.WriteLine("\nWhat would you like to do?\n1) Shortest Time Remaining First (STRF)\n2)Multi-Level Feedback Queue (MLFQ)\n3) Exit");
            userInput = Convert.ToInt32(Console.ReadLine());

            switch(userInput){
                case 1: 
                    Algorithm.strfAlgorithm();
                break;

                case 2: 
                    Algorithm.mlfqAlgorithm();
                break;

                case 3:
                    Console.WriteLine("Exiting program");
                break;
            }
        }
        
    }
}

