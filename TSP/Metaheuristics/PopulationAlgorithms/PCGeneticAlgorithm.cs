using ExCSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;
using TSP.InitialSolition.InitialAlgorithms;
using TSP.LocalSearch.IntraAlgorithms;
using TSP.Miscellaneous;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace TSP.Metaheuristics.PopulationAlgorithms
{
    public class Chromosome
    {
        /// <summary>
        /// An individual is characterized by a set of parameters (variables) known as Genes. Genes are joined into a string to form a Chromosome (solution).
        /// </summary>
        public Chromosome()
        {
            genes = new List<Vertex>();
        }
        public List<Vertex> genes { get; set; }
        public double fitness { get; set; }
        public double diversityFitness { get; set; }
        public int diversityContribution { get; set; }
        public double rating { get; set; }
    }

    public class PopulationGroup
    {
        public PopulationGroup()
        {
            groupMembers = new List<Chromosome>();
            selectionCount = 1;
        }
        public double groupDiversity { get; set; }
        public double groupAvgGlobalDiversity { get; set; }
        public int groupSize { get; set; }
        public double groupAvgFitness { get; set; }
        public double groupDiversityFitness { get; set; }
        public List<Chromosome> groupMembers { get; set; }
        public int selectionCount { get; set; }
    }

    public class Genetic
    {
        public Genetic()
        {
            ChromosomePool = new List<Chromosome>();
            random = new Random();
            customers = new List<Vertex>();
            savingsList = new List<Edge>();
            eliteChromosome = new Dictionary<int, List<Chromosome>>();
            eliteChromosome[0] = new List<Chromosome>();
            eliteChromosome[1] = new List<Chromosome>();
            eliteChromosome[2] = new List<Chromosome>();
            matingPool = new List<Chromosome>();
            chromoGroups = new Dictionary<int, PopulationGroup>();
            nrMatingPool = 150;
            maxIterations = 1500;
            maxBestNotImproved = 50;
            iterationCount = 0;
            solutionImprovedCount = 0;
            relocateLS = new IntraRelocate();
            twoOptLS = new Intra2Opt();
        }

        Random random;
        public Graph graph { get; set; }
        public List<Vertex> customers { get; set; }
        public List<Chromosome> ChromosomePool { get; set; }
        public List<Edge> savingsList;
        public Dictionary<int, List<Chromosome>> eliteChromosome { get; set; }
        public List<Chromosome> matingPool { get; set; }
        public Dictionary<int, PopulationGroup> chromoGroups { get; set; }
        public int poolSize { get; set; }
        public int nrElite { get; set; }
        public int nrMatingPool { get; set; }
        public int avgDiversityPool { get; set; }
        public double avgFitnessPool { get; set; }
        public double avgBFitnessPool { get; set; }
        public int maxIterations { get; set; }
        public int maxBestNotImproved { get; set; }
        public int crossingRate { get; set; }
        public int mutationRate { get; set; }
        public int iterationCount { get; set; }
        public int solutionImprovedCount { get; set; }
        // Create a dictionary to store the most frequent position and number of occurrences of each customer ID.
        public Dictionary<int, (int position, int occurrences)> customerIdMostFrequentPositions = new Dictionary<int, (int position, int occurrences)>();
        private IntraRelocate relocateLS { get; set; }
        private Intra2Opt twoOptLS { get; set; }
        public Chromosome bestSolution { get; set; }

        public void InitLocalSearch()
        {
            this.twoOptLS.graph = this.graph;
            this.relocateLS.graph = this.graph;
        }


        public void CustomerPositionFrequency()
        {
            // Assume that we have a list of Chromosomes called 'ChromosomePool'.

            // Create a dictionary to store the frequencies of each customer ID.
            Dictionary<int, int> customerIdFrequencies = new Dictionary<int, int>();

            // Loop through each Chromosome.
            foreach (Chromosome chromosome in this.ChromosomePool)
            {
                // Loop through each customer ID (Vertex) in the Chromosome.
                foreach (Vertex vertex in chromosome.genes)
                {
                    int customerId = vertex.index;

                    // If the customer ID is not in the dictionary, add it with a frequency of 1.
                    if (!customerIdFrequencies.ContainsKey(customerId))
                    {
                        customerIdFrequencies[customerId] = 1;
                    }
                    // If the customer ID is already in the dictionary, increment its frequency.
                    else
                    {
                        customerIdFrequencies[customerId]++;
                    }
                }
            }

            // At this point, the dictionary 'customerIdFrequencies' will contain the frequencies of each customer ID.

            // To find the most frequent position of each customer ID, we can loop through the dictionary and get the index of each customer ID in each Chromosome.

            // Create a dictionary to store the most frequent position and number of occurrences of each customer ID.
            this.customerIdMostFrequentPositions = new Dictionary<int, (int position, int occurrences)>();

            // Loop through each customer ID in the frequency dictionary.
            foreach (KeyValuePair<int, int> kvp in customerIdFrequencies)
            {
                int customerId = kvp.Key;
                int frequency = kvp.Value;

                // Create a dictionary to store the positions of this customer ID in each Chromosome.
                Dictionary<int, int> positions = new Dictionary<int, int>();

                // Loop through each Chromosome.
                for (int i = 0; i < this.ChromosomePool.Count; i++)
                {
                    Chromosome chromosome = this.ChromosomePool[i];

                    // Get the index of the customer ID in this Chromosome.
                    int index = chromosome.genes.FindIndex(x => x.index == customerId);

                    // If the customer ID was found in the Chromosome, add its index to the positions dictionary.
                    if (index != -1)
                    {
                        positions[i] = index;
                    }
                }

                // Find the position that occurs most frequently.
                int mostFrequentPosition = positions.GroupBy(x => x.Value)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .FirstOrDefault();
                
                // Find the number of occurrences at the most frequent position.
                int occurrences = positions.Count(x => x.Value == mostFrequentPosition);

                // Add the customer ID and its most frequent position and number of occurrences to the final dictionary.
                this.customerIdMostFrequentPositions[customerId] = (mostFrequentPosition, occurrences);
            }

            // At this point, the dictionary 'customerIdMostFrequentPositions' will contain the most
            int a = 0;

            this.FindBeneficialCustomerIdPairsByPosition();
        }

        public void FindBeneficialCustomerIdPairsByPosition()
        {
            // Assume that we have a list of Chromosomes called 'chromosomes' and a list of savings called 'savingsList'

            // Create a dictionary to store the customer ID pairs and their corresponding savings from the Clarke and Wright algorithm
            Dictionary<Tuple<int, int>, double> customerIdPairsSavings = new Dictionary<Tuple<int, int>, double>();

            // Loop through each Chromosome
            foreach (Chromosome chromosome in this.ChromosomePool)
            {
                // Loop through each customer ID (Vertex) in the Chromosome
                for (int i = 0; i < chromosome.genes.Count; i++)
                {
                    Vertex vertex1 = chromosome.genes[i];
                    int customerId1 = vertex1.index;

                    // Check if this customer ID is present in the customerIdMostFrequentPositions dictionary
                    if (customerIdMostFrequentPositions.ContainsKey(customerId1))
                    {
                        // Get the most frequent position of this customer ID
                        int mostFrequentPosition = customerIdMostFrequentPositions[customerId1].position;

                        // Check if the current position of this customer ID is the most frequent position
                        if (i == mostFrequentPosition)
                        {
                            // If this is the last customer ID in the Chromosome, we don't need to check for a neighboring customer ID
                            if (i == chromosome.genes.Count - 1)
                            {
                                continue;
                            }

                            // Get the next customer ID in the Chromosome
                            Vertex vertex2 = chromosome.genes[i + 1];
                            int customerId2 = vertex2.index;

                            // Check if this customer ID is present in the customerIdMostFrequentPositions dictionary
                            if (customerIdMostFrequentPositions.ContainsKey(customerId2))
                            {
                                // Get the most frequent position of this customer ID
                                int mostFrequentPosition2 = customerIdMostFrequentPositions[customerId2].position;

                                // Check if the next position in the Chromosome is the most frequent position for this customer ID
                                if (i + 1 == mostFrequentPosition2)
                                {
                                    // Check if this customer ID pair is present in the savings list
                                    Edge savings = savingsList.Find(x => (x.vertex1.index == customerId1 && x.vertex2.index == customerId2) ||
                                                                        (x.vertex1.index == customerId2 && x.vertex2.index == customerId1));
                                    if (savings != null)
                                    {
                                        // Check if the customer IDs are directly connected
                                        if (savings.vertex1.index == customerId1 && savings.vertex2.index == customerId2)
                                        {
                                            // Add the customer ID pair and its savings to the dictionary
                                            customerIdPairsSavings[Tuple.Create(customerId1, customerId2)] = savings.distance;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // At this point, the dictionary 'customerIdPairsSavings' will contain the customer ID pairs and their corresponding savings from the Clarke and Wright algorithm

            // You can then loop through the dictionary and highlight the customer ID pairs with high savings for further analysis
            //foreach (KeyValuePair<Tuple<int, int>, double> kvp in customerIdPairsSav
        }

        /// <summary>
        /// Generate a initial pool of Chromosomes. Each individual is a solution to the TSP problem.
        /// </summary>
        public void GenerateChromosomes(int poolSize)
        {
            this.CWSavingsList();
            this.poolSize = poolSize;
            this.nrMatingPool = poolSize * 2;
            this.nrElite = (int)Math.Ceiling(nrMatingPool * 0.1);

            this.customers = this.graph.vertices.Values.ToList(); // Only one depot is present in TSP.
            this.customers.Remove(this.customers[0]);  // Remove the depot at index 0.

            this.FillChromosomePool(poolSize);

            this.PopulationDiversityUpdate();
            this.GroupPopulationByFitness();
            this.MatingPoolSelection();
        }

        /// <summary>
        /// Fills the chromosome pool to the specified size with randomly generated chromosomes.
        /// </summary>
        /// <param name="poolSize">The desired size of the chromosome pool.</param>
        public void FillChromosomePool(int poolSize)
        {
            while (this.ChromosomePool.Count != poolSize)
            {
                Chromosome tempChromosome = new Chromosome();

                int initSolution = this.random.Next(13);

                // Random order of vertices
                if (initSolution >= 0 && initSolution < 4)
                {
                    tempChromosome.genes = this.customers
                        .OrderBy(item => this.random.Next())
                        .ToList();

                    // Exclude the depot vertex from mutation
                    tempChromosome.genes.Remove(this.graph.vertices[0]);
                }
                // NNH algorithm - stochastic
                else if (initSolution >= 4 && initSolution <= 8)
                tempChromosome = this.NNHStochastic(tempChromosome);
                // CW algorithm - stochastic
                else if (initSolution >= 9)
                    tempChromosome = this.CWStohastic(tempChromosome);

                // Evaluate fitness
                tempChromosome.fitness = this.Fitness(tempChromosome);

                // Skip invalid chromosomes
                if (tempChromosome.fitness == -1)
                    continue;

                this.ChromosomePool.Add(tempChromosome);
            }
        }

        /// <summary>
        /// Average fitness value of the population.
        /// </summary>
        public void PopulationFitness()
        {
            this.avgFitnessPool = this.ChromosomePool.Average(x => x.fitness);
            this.avgBFitnessPool = this.ChromosomePool.Average(x => x.diversityFitness);
        }

        /// <summary>
        /// Average diversity of each solution to all other solutions.
        /// </summary>
        public void PopulationDiversityUpdate()
        {
            try
            {
                this.ChromosomePool = this.ChromosomePool.OrderBy(x => x.fitness).Select(x => x).ToList();
            }
            catch (Exception)
            {
                this.ChromosomePool = this.ChromosomePool.OrderBy(x => x.fitness).Select(x => x).ToList();
            }

            int populationSize = this.ChromosomePool.Count;
            int globalHammingDistance = 0;

            foreach (Chromosome chromo in this.ChromosomePool)
            {
                int avgHD = 0;

                foreach (var chromo_2 in this.ChromosomePool)
                {
                    if (chromo == chromo_2)
                        continue;
                    avgHD += MiscMath.GetHammingDistance(chromo.genes, chromo_2.genes);
                }

                avgHD /= (populationSize - 1);
                chromo.diversityContribution = avgHD;
                globalHammingDistance += avgHD;

                // Calculate the diversity adjusted fitness value of each Chromosome.
                // Higher diversity yields a lower adjustment of the fitness -> More diverse chromosom = better Chromosome.
                double chromoDiversityFunction = chromo.fitness * (1 - ((double)chromo.diversityContribution / (double)this.customers.Count));
                chromo.diversityFitness = chromo.fitness + chromoDiversityFunction;
            }

            this.avgDiversityPool = globalHammingDistance / populationSize;

            this.PopulationFitness();
        }

        /// <summary>
        /// Calculate the fitness/evaluation value of the solution (Chromosome). E.g. the total distance of the path (Chromosome).
        /// The fitness function determines how fit an individual is (the ability of an individual to compete with other individuals). 
        /// It gives a fitness score to each individual. The probability that an individual will be selected for reproduction is based on its fitness score.
        /// </summary>
        public double Fitness(Chromosome Chromosome)
        {
            // Add the depot to first and last index of the path to get a correct value of the path cost.
            Chromosome.genes.Insert(0, this.graph.vertices[0]);
            Chromosome.genes.Add(this.graph.vertices[0]);

            double fitness = GraphMethods.PathDistanceCost(Chromosome.genes);

            // Remove the depot to evade it being mutated.
            Chromosome.genes.Remove(this.graph.vertices[0]);
            Chromosome.genes.Remove(this.graph.vertices[0]);

            return fitness;
        }

        /// <summary>
        /// Builds a Chromosome by selecting a gene at index `i + 1` via stochastic NNH (Nearest Neighbor Heuristic).
        /// For each gene at index `i`, selects the top 5 nearest neighbors that are not present in the current Chromosome.
        /// The probability of a neighbor being selected is equal to its slightly adjusted fitness value, i.e. its distance to the node at index `i`.
        /// </summary>
        /// <returns>The constructed Chromosome</returns>
        public Chromosome NNHStochastic(Chromosome tempChromosome)
        {
            List<Vertex> tempGenes = new List<Vertex>
            {
                this.graph.vertices[0]  // Start vertex is the depot.
            };

            // Loop until all vertices are added to tempGenes list
            while (tempGenes.Count != this.graph.vertices.Count)
            {
                Vertex lastVertex = tempGenes.Last();
                List<Vertex> nnhList = new List<Vertex>();
                List<double> nnhFitness = new List<double>();

                // Get top 5 nearest neighbors that are not already in tempGenes list
                foreach (var item in lastVertex.neighbors.Where(x => !tempGenes.Contains(x.Value.vertex2))
                    .GroupBy(x => x.Value.distance).SelectMany(x => x).OrderBy(x => x.Value.distance).Take(5)
                    .ToDictionary(x => x.Value.vertex2).ToList())
                {
                    nnhList.Add(item.Value.Value.vertex2);
                    nnhFitness.Add(item.Value.Value.distance);
                }

                // Adjust fitness value for each neighbor to determine probability
                nnhFitness = MiscMath.DeterminismFactorScaling(nnhFitness, this.customers.Count);

                // Spin the wheel to determine the selected vertex
                int wheelSpin = this.random.Next(101);
                int spinResultIndex = 0;
                double cumulative = 0.0;
                for (int i = nnhFitness.Count - 1; i >= 0; i--)
                {
                    cumulative += nnhFitness[i];
                    if (wheelSpin < cumulative)
                    {
                        spinResultIndex = i;
                        break;
                    }
                }

                // Add selected vertex to tempGenes list
                tempGenes.Add(nnhList[spinResultIndex]);
            }

            // Remove depot from tempGenes to prevent mutation
            tempGenes.Remove(this.graph.vertices[0]);

            // Update tempChromosome genes
            tempChromosome.genes.Clear();
            tempChromosome.genes.AddRange(tempGenes);

            return tempChromosome;
        }

        /// <summary>
        /// Construct a Chromosome by selecting the first - 1 or last + 1 gene via stohastic CW.
        /// From the CW savings list select top 5 edges that comply with the restrictions.
        /// The probability of the neighbor being selected is equal to its slightly adjusted fitness value.
        /// </summary>
        public Chromosome CWStohastic(Chromosome tempChromosome)
        {
            List<Vertex> tempGenes = new List<Vertex>();

            // Select first vertices pair randomly for the top 5.
            Edge tempEdge = this.savingsList[this.random.Next(6)];
            tempGenes.Add(tempEdge.vertex1);
            tempGenes.Add(tempEdge.vertex2);

            while (tempGenes.Count != this.graph.vertices.Count - 1)
            {
                // Create a list of top 5 CW.
                List<Edge> cwList = new List<Edge>();
                List<double> cwFitness = new List<double>();
                int top5Counter = 0;

                // Select top 5 highest savings edges that have either the vertex2 same as the tempEdge vertex1 or the vertex1 equal to tempEdge vertex2.
                foreach (var item in this.savingsList.Where(x => x.vertex1 == tempEdge.vertex2 || x.vertex2 == tempEdge.vertex1)
                    .GroupBy(x => x.distance).SelectMany(x => x).OrderBy(x => x.distance).ToDictionary(x => x).ToList())
                {
                    Vertex v1 = item.Value.vertex1;  // Start vertex of the edge.
                    Vertex v2 = item.Value.vertex2;  // End vertex of the edge.

                    bool v1InTour = tempGenes.Contains(v1), v2InTour = tempGenes.Contains(v2);

                    if (v1InTour && v2InTour)
                        continue;

                    top5Counter++;

                    if (top5Counter == 5)
                        break;

                    cwList.Add(item.Value);
                    cwFitness.Add(item.Value.distance);
                }

                // The probability of the Edge to be selected:
                cwFitness = MiscMath.DeterminismFactorScaling(cwFitness, this.customers.Count);

                int wheelSpin = this.random.Next(101);
                int spinResultIndex = 0;

                double cumulative = 0.0;
                for (int i = cwFitness.Count - 1; i >= 0; i--)
                {
                    cumulative += cwFitness[i];
                    if (wheelSpin < cumulative)
                    {
                        spinResultIndex = i;
                        break;
                    }
                }

                if (cwList.Count == 0)
                {
                    var lastVertex = this.graph.vertices.Where(x => !tempGenes.Contains(x.Value)).Select(x => x).ToList().FirstOrDefault();
                    cwList.Add(this.graph.edges[Tuple.Create(lastVertex.Value.index, tempGenes.First().index)]);
                }

                // The edge must either have the first vertex in the path as the v2 in the edge: x -> first element : (go to the first vertex in the temp path).
                // Or the last vertex in the path as the v1 in the edge: last element -> x : (lead out of the last vertex in the temp path).
                // e.g. temp path [2, 5], temp edge [1, 3] -> 3 != 2 and 1 != 5 -> we cant connect the edge to the temp path, skip.

                bool tempV1ToFirstVertex = cwList[spinResultIndex].vertex2 == tempGenes.First();
                bool tempV2OutOfLastVertex = cwList[spinResultIndex].vertex1 == tempGenes.Last();

                // If the vertices of the current edge can not be connected to the first or last elements of the path, skip
                if (!tempV1ToFirstVertex && !tempV2OutOfLastVertex)
                    continue;

                if (tempV1ToFirstVertex)
                    tempGenes.Insert(0, cwList[spinResultIndex].vertex1);
                else if (tempV2OutOfLastVertex)
                    tempGenes.Add(cwList[spinResultIndex].vertex2);

                // New tempEdge = first and last element in the gene sequence.
                tempEdge.vertex1 = tempGenes.First();
                tempEdge.vertex2 = tempGenes.Last();
            }

            // Remove the depot to evade it being mutated.
            tempGenes.Remove(this.graph.vertices[0]);

            tempChromosome.genes.Clear();
            tempChromosome.genes.AddRange(tempGenes);

            return tempChromosome;
        }

        /// <summary>
        /// Generate a CW savings list from the provided graph. The list will be used for CW operations in genes.
        /// </summary>
        public void CWSavingsList()
        {
            Vertex depot = this.graph.vertices[0];

            // For each Neighbor of the current Depot -> We want to create a list of savings that concern only the current Depot.
            foreach (KeyValuePair<Tuple<int, int>, Edge> depotNeighbor in depot.neighbors)
            {
                // The current Vertex is the neighbor of the current Depot.
                Vertex vertex = depotNeighbor.Value.vertex2;

                // For each neighbor of a vertex that is connected to the current Depot.
                foreach (KeyValuePair<Tuple<int, int>, Edge> neighbor in vertex.neighbors)
                {
                    //// Skip if the Edge end node is a depot.
                    if (neighbor.Value.vertex2 == depot)
                        continue;

                    // S(i,j) = d(depot,i) + d(j,depot) - d(i,j).
                    double cwCostSaving = this.graph.edges[Tuple.Create(depot.index, neighbor.Value.vertex1.index)].distance +
                        this.graph.edges[Tuple.Create(neighbor.Value.vertex2.index, depot.index)].distance - neighbor.Value.distance;

                    Edge validEdge = new Edge { vertex1 = neighbor.Value.vertex1, vertex2 = neighbor.Value.vertex2, distance = cwCostSaving };
                    this.savingsList.Add(validEdge);
                }
            }

            // Sort the list by the savings value - Descending.
            this.savingsList = this.savingsList.OrderByDescending(x => x.distance).ToList();
        }

        /// <summary>
        /// Group the population into N groups. A Chromosome belongs to a group if its fitness difference value to the average
        /// fitness value of the group is less than 10% of the best fitness value in the complete generation.
        /// </summary>
        public void GroupPopulationByFitness()
        {
            // Create a list all Chromosome fitness in the pool ranked by the ascending fitness parameter.
            // The pool is divided into N groups of Chromosomes usling k-means-like approach.
            // The pool must be pre-sorted by ascending fitness value.
            this.ChromosomePool = this.ChromosomePool.OrderBy(x => x.fitness).Select(x => x).ToList();

            double difference = this.ChromosomePool[0].fitness * 0.1;

            Dictionary<int, PopulationGroup> groups = new Dictionary<int, PopulationGroup>();
            int groupID = 0;
            List<Chromosome> tempGroup = new List<Chromosome>
            {
            this.ChromosomePool.First()
            };

            for (int i = 1; i < this.ChromosomePool.Count - 1; i++)
            {
                // Set group limit to 10 solutions.
                if (tempGroup.Count == 10)
                {
                    groupID = groupID;  // Used as a filler to continue.
                }
                // Check if the element i is within a "difference" range from the average element value of the group, if so, add it to the group.
                else if (Math.Abs(this.ChromosomePool[i].fitness - tempGroup.Average(x => x.fitness)) < difference)
                {
                    tempGroup.Add(this.ChromosomePool[i]);
                    continue;
                }

                // Element has surpassed the allowed "difference" range, add it to the next group.
                groups[groupID] = CreatePopulationGroup(tempGroup);

                groupID++;
                tempGroup.Clear();
                tempGroup.Add(this.ChromosomePool[i]);
                continue;
            }

            // Add last group.
            tempGroup.Add(this.ChromosomePool.Last());

            groups[groupID] = CreatePopulationGroup(tempGroup);

            this.chromoGroups = groups;

            this.SelectEliteFromPopulationGroup();
        }

        public PopulationGroup CreatePopulationGroup(List<Chromosome> tempGroup)
        {
            PopulationGroup pGroup = new PopulationGroup();
            int gHD = 0;

            // Get Group average difference.
            foreach (Chromosome chromo in tempGroup)
            {
                int avgHD = 0;

                foreach (var chromo_2 in tempGroup)
                {
                    if (chromo == chromo_2)
                        continue;
                    avgHD += MiscMath.GetHammingDistance(chromo.genes, chromo_2.genes);
                }

                if (tempGroup.Count > 1)
                    avgHD /= (tempGroup.Count - 1);
                gHD += avgHD;
            }
            gHD /= tempGroup.Count;

            pGroup = new PopulationGroup();
            pGroup.groupMembers.AddRange(tempGroup);
            pGroup.groupSize = tempGroup.Count;
            pGroup.groupDiversity = gHD;
            pGroup.groupAvgGlobalDiversity = tempGroup.Select(x => x.diversityContribution).Average();
            pGroup.groupAvgFitness = tempGroup.Select(x => x.fitness).Average();

            // Group Diversity Fitness aims to increase the chance of smaller group with higher diversity to be selected for mating. 
            // Persumption is: 1) Smaller group = more unique solution space -> The formula will add a smaller penalty to the existing fitness.
            // 2) 1 - (Group Div / Total vertex count) -> The penaly decreases proportionaly with the diversity of the group.
            gHD = (gHD == 0) ? this.customers.Count - 1 : gHD;
            double groupDiversityFunction = ((double)pGroup.groupSize / (double)this.ChromosomePool.Count) * (double)pGroup.groupAvgFitness;
            groupDiversityFunction *= (double)1 - ((double)pGroup.groupDiversity / (double)this.customers.Count);
            pGroup.groupDiversityFitness = pGroup.groupAvgFitness + groupDiversityFunction;

            return pGroup;
        }

        /// <summary>
        /// Total elite Chromosome = totalPopulation.Count * 0.1.
        /// The elite Chromosomes are divided into three groups. Each iteration old elites are compared to new elites per group.
        /// Distribution of elite candidate Chromosomes from the current population groups:
        /// First 45% are Chromosomes that have the highest fitenss value.
        /// Second 35% are the ones that have the highest diversity fitness value in the second group, having that this subgroup element must not be present in the first 45% subgroup.
        /// Last 20% are Chromosomes from the end of the list (worst average fitness) with the highest diversity contribution.
        /// It is persumed that the last groups contain solutions that will maintain the diversity of the Genetic pool.
        /// </summary>
        public void SelectEliteFromPopulationGroup()
        {
            int takeCount = (int)Math.Ceiling(this.nrElite * 0.45);

            // Get first 45%.
            // If first group new elites have higher fitness value than old first group elites, replace them.
            // Add new elites to first group.
            List<Chromosome> firsGroup = this.chromoGroups[0].groupMembers.GroupBy(x => x.fitness).Select(x => x.FirstOrDefault())
                .Where(x => !this.eliteChromosome[0].Select(s => (int)s.fitness).ToList().Contains((int)x.fitness)).OrderBy(x => x.fitness).ToList();
            this.eliteChromosome[0].AddRange(firsGroup);
            // Take only the fittest of the new combined group.
            this.eliteChromosome[0] = this.eliteChromosome[0].Select(x => x).OrderBy(x => x.fitness).Take(takeCount).ToList();

            // Get second 35%.
            List<Chromosome> secondGroup = this.chromoGroups[1].groupMembers.GroupBy(x => x.fitness).Select(x => x.FirstOrDefault())
                .Where(x => !this.eliteChromosome[1].Select(s => (int)s.fitness).ToList().Contains((int)x.fitness) && !firsGroup.Select(s => (int)s.fitness).ToList().Contains((int)x.fitness))
                .OrderBy(x => x.fitness).ToList();
            takeCount = (int)Math.Ceiling(this.nrElite * 0.35);
            this.eliteChromosome[1].AddRange(secondGroup);
            this.eliteChromosome[1] = this.eliteChromosome[1].Select(x => x).OrderBy(x => x.fitness).Take(takeCount).ToList();

            // Last 20%.
            takeCount = (int)Math.Ceiling(this.nrElite * 0.2);
            List<Chromosome> thirdGroup = this.ChromosomePool.GroupBy(x => x.fitness).Select(x => x.FirstOrDefault())
                .Where(x => !this.eliteChromosome[2].Select(s => (int)s.fitness).ToList().Contains((int)x.fitness)
                && !secondGroup.Select(s => (int)s.fitness).ToList().Contains((int)x.fitness)
                && !firsGroup.Select(s => (int)s.fitness).ToList().Contains((int)x.fitness))
                .OrderByDescending(x => x.fitness).ToList();
            this.eliteChromosome[2].AddRange(firsGroup);
            this.eliteChromosome[2] = this.eliteChromosome[2].Select(x => x).OrderByDescending(x => x.fitness).Take(takeCount).ToList();
        }

        /// <summary>
        /// Select guided random mating pairs from population groups.
        /// </summary>
        public void MatingPoolSelection()
        {
            this.matingPool.Clear();

            while (this.matingPool.Count <= this.nrMatingPool)
            {
                // TODO: If group selection is frequent, decrease the odds so that other groups get a chance.

                // Random select two groups. Group 1 will donate parent 1, group 2 will donate parent 2.
                List<double> groupProbability = this.chromoGroups.Select(x => x.Value.groupAvgFitness).ToList();
                groupProbability = MiscMath.DeterminismFactorScaling(groupProbability, 1, 9.5);

                int parentOneWheelSpin = this.random.Next(101);
                int parentTwoWheelSpin = this.random.Next(101);
                int parentOneSpinResultIndex = -1;
                int parentTwoSpinResultIndex = -1;

                double cumulative = 0.0;
                for (int i = groupProbability.Count - 1; i >= 0; i--)
                {
                    cumulative += groupProbability[i];
                    if (parentOneSpinResultIndex == -1)
                    {
                        if (parentOneWheelSpin < cumulative)
                        {
                            parentOneSpinResultIndex = i;
                        }
                    }
                    if (parentTwoSpinResultIndex == -1)
                    {
                        if (parentTwoWheelSpin < cumulative)
                        {
                            parentTwoSpinResultIndex = i;
                        }
                    }
                    if (parentOneSpinResultIndex != -1 && parentTwoSpinResultIndex != -1)
                        break;
                }

                if (parentOneSpinResultIndex == -1)
                    parentOneSpinResultIndex = 0;
                if (parentTwoSpinResultIndex == -1)
                    parentTwoSpinResultIndex = 0;

                // Selected groups:
                PopulationGroup parentOneGroup = this.chromoGroups[parentOneSpinResultIndex];
                this.chromoGroups[parentOneSpinResultIndex].selectionCount++;
                PopulationGroup parentTwoGroup = this.chromoGroups[parentTwoSpinResultIndex];
                this.chromoGroups[parentTwoSpinResultIndex].selectionCount++;

                List<double> childOneProbability = new List<double>();
                List<double> childTwoProbability = new List<double>();

                if (this.matingPool.Count > this.nrMatingPool * 0.30)
                {
                    // Check pool diversity.

                    double averageMatingPoolDiversity = 0;
                    averageMatingPoolDiversity += this.matingPool.Average(x => x.diversityContribution);
                    averageMatingPoolDiversity += this.matingPool.Average(x => x.diversityContribution);
                    averageMatingPoolDiversity /= 2;
                    averageMatingPoolDiversity = (double)(averageMatingPoolDiversity / this.customers.Count) * 100;

                    // If average mating pool diversity is less than 60% -> aim to pick Chromosomes with greater diversity.
                    if (averageMatingPoolDiversity < 60)
                    {
                        parentOneGroup.groupMembers = parentOneGroup.groupMembers.Select(x => x).OrderByDescending(x => x.diversityContribution).ToList();
                        parentTwoGroup.groupMembers = parentTwoGroup.groupMembers.Select(x => x).OrderByDescending(x => x.diversityContribution).ToList();
                        childOneProbability = parentOneGroup.groupMembers.Select(x => Convert.ToDouble(x.diversityContribution)).ToList();
                        childTwoProbability = parentTwoGroup.groupMembers.Select(x => Convert.ToDouble(x.diversityContribution)).ToList();
                    }

                    // Else aim to pick Chromosomes with lower diversity fitness -> better objective function value.
                    else
                    {
                        childOneProbability = parentOneGroup.groupMembers.Select(x => x.diversityFitness).ToList();
                        childTwoProbability = parentTwoGroup.groupMembers.Select(x => x.diversityFitness).ToList();
                    }
                }
                else
                {
                    childOneProbability = parentOneGroup.groupMembers.Select(x => x.diversityFitness).ToList();
                    childTwoProbability = parentTwoGroup.groupMembers.Select(x => x.diversityFitness).ToList();
                }

                childOneProbability = MiscMath.DeterminismFactorScaling(childOneProbability, 1);
                childTwoProbability = MiscMath.DeterminismFactorScaling(childTwoProbability, 1);

                // Select Child 1 and Child 2.

                int childOneWheelSpin = this.random.Next(101);
                int childTwoWheelSpin = this.random.Next(101);
                int childOneIndex = 0;
                int childTwoIndex = 0;

                cumulative = 0.0;

                for (int i = 0; i < childOneProbability.Count - 1; i++)
                {
                    cumulative += childOneProbability[i];
                    if (childOneWheelSpin < cumulative)
                    {
                        childOneIndex = i;
                        break;
                    }
                }

                cumulative = 0.0;

                for (int i = 0; i < childTwoProbability.Count - 1; i++)
                {
                    cumulative += childTwoProbability[i];
                    if (childTwoWheelSpin < cumulative)
                    {
                        childTwoIndex = i;
                        break;
                    }
                }

                // Add parents to mating pool.
                this.matingPool.Add(parentOneGroup.groupMembers[childOneIndex]);
                this.matingPool.Add(parentTwoGroup.groupMembers[childTwoIndex]);
            }

            // If any of the elite Chromosomes is not in the mating pool, add them to a random place.
            foreach (KeyValuePair<int, List<Chromosome>> eliteGroup in this.eliteChromosome)
            {
                foreach (Chromosome member in eliteGroup.Value)
                {
                    if (!this.matingPool.Contains(member))
                        this.matingPool.Insert(this.random.Next(0, this.matingPool.Count - 1), member);
                }
            }
        }

        /// <summary>
        /// Executes a partially matched crossover (PMX) on the input individuals.
        /// PMX generates two children by matching pairs of values in a certain range of the two parents and swapping the values of those indexes.
        /// For more details see Goldberg and Lingel, "Alleles, loci, and the traveling salesman problem", 1985.
        /// </summary>
        public List<Chromosome> PartiallyMappedCrossover(List<Vertex> parent1, List<Vertex> parent2)
        {
            try
            {
                while (parent1.Contains(this.graph.vertices[0]))
                    parent1.Remove(this.graph.vertices[0]);
                while (parent2.Contains(this.graph.vertices[0]))
                    parent2.Remove(this.graph.vertices[0]);

                int ChromosomeCount = parent1.Count;

                // Initialise children as arrays.
                Vertex[] child1 = new Vertex[ChromosomeCount];
                Vertex[] child2 = new Vertex[ChromosomeCount];

                // Generate 2 random cut points (crossover points). cut1 != cut2
                int cx1 = this.random.Next(0, ChromosomeCount - 2);
                int cx2 = this.random.Next(cx1 + 1, ChromosomeCount - 1);

                // Transfer parent genes from crossSection to children.
                for (int i = cx1; i <= cx2; i++)
                {
                    child1[i] = parent1[i];
                    child2[i] = parent2[i];
                }

                // Fill missing elements in children. Parent1 donates genes to Child 2, Parent2 Child1.
                for (int i = 0; i < ChromosomeCount - 1; i++)
                {
                    // Child 1.
                    if (child1[i] == null)
                    {
                        if (!child1.Contains(parent2[i]))
                        {
                            child1[i] = parent2[i];
                        }
                        // Not feasable. Recursively find a feasable element.
                        else
                        {
                            child1[i] = child2[this.RecursivePMXFindFeasableGene(child1.ToList(), child2.ToList(), parent2[i])];
                        }
                    }

                    // Child 2.
                    if (child2[i] == null)
                    {
                        if (!child2.Contains(parent1[i]))
                        {
                            child2[i] = parent1[i];
                        }
                        else
                        {
                            child2[i] = child1[this.RecursivePMXFindFeasableGene(child2.ToList(), child1.ToList(), parent1[i])];
                        }
                    }
                }

                // Add last missing element in c1 and c2.
                if (child1[ChromosomeCount - 1] == null)
                    child1[ChromosomeCount - 1] = this.customers.Select(x => x).Where(x => !child1.Contains(x)).FirstOrDefault();
                if (child2[ChromosomeCount - 1] == null)
                    child2[ChromosomeCount - 1] = this.customers.Select(x => x).Where(x => !child2.Contains(x)).FirstOrDefault();

                // Return and add children to new population pool.
                Chromosome childOne = new Chromosome();
                Chromosome childTwo = new Chromosome();
                childOne.genes = child1.ToList();
                childTwo.genes = child2.ToList();
                childOne.fitness = this.Fitness(childOne);
                childTwo.fitness = this.Fitness(childTwo);

                return new List<Chromosome> { childOne, childTwo };
            }
            catch (Exception ex)
            {
                return new List<Chromosome>();
            }
        }

        public int RecursivePMXFindFeasableGene(List<Vertex> targetChild1, List<Vertex> child2, Vertex tempNotFeasableElement, int lastCheckedIndex=0, int recursionCounter=0)
        {
            if (recursionCounter > targetChild1.Count * 2)
                return child2.IndexOf(child2.Where(x => !targetChild1.Contains(x)).FirstOrDefault());

            int feasableIndex = targetChild1.IndexOf(tempNotFeasableElement);

            if (feasableIndex == -1)
            {
                if (!targetChild1.Contains(tempNotFeasableElement))
                    return child2.IndexOf(tempNotFeasableElement);
            }

            if (!targetChild1.Contains(child2[feasableIndex]))
                return feasableIndex;

            tempNotFeasableElement = child2[feasableIndex];

            // If child1 and child2 have same Vertex at same index, infinite loop will occure.
            if (lastCheckedIndex == feasableIndex)
            {
                if (feasableIndex < targetChild1.Count - 1)
                    feasableIndex++;
                else
                    feasableIndex--;
                //If it would still for a loop, e.g.c1(4, 1), c2(1, 4)->Possible inifinite loop.
                if (targetChild1.Contains(child2[feasableIndex]))
                    tempNotFeasableElement = child2[this.random.Next(0, child2.Count - 1)];
            }

            // e.g. c1(4,1), c2(1,4) -> Possible inifinite loop.
            if (Math.Abs(feasableIndex - lastCheckedIndex) == 1)
            {
                if (targetChild1.Contains(child2[feasableIndex]))
                    tempNotFeasableElement = child2[this.random.Next(0, child2.Count - 1)];
            }

            return this.RecursivePMXFindFeasableGene(targetChild1, child2, tempNotFeasableElement, feasableIndex, recursionCounter=recursionCounter + 1);
        }

        /// <summary>
        /// Perform PMX crossing for each mating pair.
        /// </summary>
        public void CrossMatingPairs()
        {
            List<Chromosome> newGeneration = new List<Chromosome>();

            for (int i = 0; i < this.matingPool.Count - 2; i+=2)
            {
                newGeneration.AddRange(this.PartiallyMappedCrossover(this.matingPool[i].genes, this.matingPool[i + 1].genes));
            }          

            // Delete current ChromosomePool population and repalce with new generation.
            this.ChromosomePool.Clear();
            this.ChromosomePool.AddRange(newGeneration);
        }

        /// <summary>
        /// Mutation occurs to maintain diversity within the population and prevent premature convergence.
        /// This operator has a 70:30 % chance to run a random Clark and Wright savings mutation or a random mutation gene switch.
        /// </summary>
        public void Mutation(Chromosome Chromosome)
        {
            // Select a random customer ID pair from the dictionary with high savings.
            int numberOfItems = (int)(this.savingsList.Count * 0.05);
            int randomIndex = random.Next(numberOfItems);
            var randomCustomerIdPair = this.savingsList.OrderByDescending(x => x.distance).Skip(randomIndex).FirstOrDefault();

            // Get the customer IDs from the tuple
            int customerId1 = randomCustomerIdPair.vertex1.index;
            int customerId2 = randomCustomerIdPair.vertex2.index;

            // Find the indices of the customer IDs in the Chromosome
            int index1 = Chromosome.genes.FindIndex(x => x.index == customerId1);
            int index2 = Chromosome.genes.FindIndex(x => x.index == customerId2);

            // Check if the customer IDs are not already directly connected
            if (index1 + 1 != index2 && index2 + 1 != index1)
            {
                try
                {
                    // Directly connect the customer IDs by removing the customer ID in between and inserting it at the correct position
                    if (index1 < index2)
                    {
                        Vertex replaceVertex = Chromosome.genes[index2];
                        Chromosome.genes.RemoveAt(index2);
                        Chromosome.genes.Insert(index1 + 1, replaceVertex);
                    }
                    else
                    {
                        Vertex replaceVertex = Chromosome.genes[index1];
                        Chromosome.genes.RemoveAt(index1);
                        Chromosome.genes.Insert(index2 + 1, replaceVertex);
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
            else
            {
                // Select two random genes.
                int a = random.Next(0, Chromosome.genes.Count - 1);
                int b = random.Next(0, Chromosome.genes.Count - 1);
                if (a == b)
                {
                    b++;
                }

                if (this.random.Next(10) >= 3)
                {
                    // CW Savings list mutation.
                    Edge tempEdgeA = this.savingsList.Select(x => x).Where(x => x.vertex1.index == a).FirstOrDefault();
                    Edge tempEdgeB = this.savingsList.Select(x => x).Where(x => x.vertex2.index == b).FirstOrDefault();

                    try
                    {
                        if (tempEdgeA != null)
                        {
                            if (Chromosome.genes[Chromosome.genes.IndexOf(tempEdgeA.vertex1) + 1] != tempEdgeA.vertex2)
                            {
                                Chromosome.genes.Remove(tempEdgeA.vertex2);
                                Chromosome.genes.Insert(Chromosome.genes.IndexOf(tempEdgeA.vertex1) + 1, tempEdgeA.vertex2);
                            }
                        }
                        if (tempEdgeB != null)
                        {
                            if (Chromosome.genes[Chromosome.genes.IndexOf(tempEdgeB.vertex2) - 1] != tempEdgeB.vertex1)
                            {
                                Chromosome.genes.Remove(tempEdgeB.vertex1);
                                Chromosome.genes.Insert(Chromosome.genes.IndexOf(tempEdgeB.vertex2) - 1, tempEdgeB.vertex1);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                else
                {
                    // Switch locations of vertices a and b.
                    Vertex tempVer = Chromosome.genes[a];
                    Chromosome.genes[a] = Chromosome.genes[b];
                    Chromosome.genes[b] = tempVer;
                }
            }
        }

        /// <summary>
        /// Random mutate new generation Chromosomes.
        /// </summary>
        public void MutateNewPopulation(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                int tagetChromosome = this.random.Next(0, this.ChromosomePool.Count - 1);
                this.Mutation(this.ChromosomePool[tagetChromosome]);
            }
        }

        /// <summary>
        /// If best solution did not improve x iterations, diversify the population by
        /// preserving top best solution from first 5 population groups and fill rest of
        /// the population with new individuals as in the initialization phase.
        /// This process aims to introduce a significant amount of new Genetic material.
        /// </summary>
        public void GlobalDiversificationOfPopulation()
        {
            // Clear population pool.
            this.ChromosomePool.Clear();

            // Add best fitness value solution from each group to the population pool. Take top 4 from first group.
            foreach (KeyValuePair<int, PopulationGroup> group in this.chromoGroups)
            {
                if (group.Key >= 5)
                    break;
                int takeValue = 1;
                if (group.Key == 0)
                    takeValue = 4;
                this.ChromosomePool.AddRange(group.Value.groupMembers.Select(x => x).OrderByDescending(x => x.fitness).Take(takeValue).ToList());
            }

            // Add top best elite Chromosomes per group.
            foreach (KeyValuePair<int, List<Chromosome>> group in this.eliteChromosome)
            {
                int takeValue = 1;
                if (group.Key == 0)
                    takeValue = 10;
                this.ChromosomePool.AddRange(group.Value.Select(x => x).OrderByDescending(x => x.fitness).Take(takeValue).ToList());
            }

            // Fill rest of Chromosome pool.
            this.FillChromosomePool(this.poolSize);
        }

        public void LocalSearchImproveSolution()
        {
            // Iterate through each group in the chromoGroups dictionary
            foreach (KeyValuePair<int, PopulationGroup> group in this.chromoGroups)
            {
                // Get the top 20% solutions in the group
                List<Chromosome> tempGroup = group.Value.groupMembers.OrderByDescending(x => x.fitness)
                                                   .Take((int)Math.Ceiling(group.Value.groupSize * 0.2)).ToList();

                // Improve the solutions in the tempGroup
                this.ImproveSolutions(tempGroup);
            }
        }

        public void ImproveSolutions(List<Chromosome> targetGroup)
        {
            // Loop through the solutions in the target group
            foreach (Chromosome item in targetGroup)
            {
                double initialFitness = item.fitness;
                // Remove the solution from the pool
                this.ChromosomePool.Remove(item);

                // Add depot to the solution
                item.genes.Insert(0, this.graph.vertices[0]);
                item.genes.Add(this.graph.vertices[0]);

                // Convert the solution to a string and store it
                string path = GetSolutionPathString(item.genes);
                Properties.Settings.Default.lastPath = path;
                Properties.Settings.Default.Save();

                // Enhance the solution with the relocateLS operator
                List<Vertex> relocatedSolution = this.relocateLS.IntraRelocateOptimization();
                path = GetSolutionPathString(relocatedSolution);
                Properties.Settings.Default.lastPath = path;
                Properties.Settings.Default.Save();

                // Enhance the solution with the twoOptLS operator
                Chromosome finalSolution = new Chromosome
                {
                    genes = this.twoOptLS.Intra2OptOptimization()
                };
                finalSolution.genes.Remove(this.graph.vertices[0]);
                finalSolution.genes.Remove(this.graph.vertices[0]);

                // Calculate the fitness of the final solution
                finalSolution.fitness = this.Fitness(finalSolution);

                // Create a deep copy of the final solution
                Chromosome deepCopyFinalSolution = new Chromosome
                {
                    genes = new List<Vertex>(finalSolution.genes),
                    fitness = finalSolution.fitness
                };

                // Add the deep copy of the final solution to the pool
                this.ChromosomePool.Add(deepCopyFinalSolution);
            }
        }

        private string GetSolutionPathString(List<Vertex> solution)
        {
            string path = String.Empty;
            int counter = 0;

            // Convert the solution to a string format
            foreach (Vertex vertex in solution)
            {
                if (counter == 0)
                    path += vertex.index;
                else
                    path += " --> " + vertex.index;
                counter++;
            }

            return path;
        }

        /// <summary>
        /// Run GA algorithm with x=iterations generations.
        /// If the best solution does not change within maxBestNotImproved generations, stop the algorithm.
        /// </summary>
        public List<Vertex> GeneticOptimization()
        {
            int generationsWithoutBestFitImprovement = 0;
            bestSolution = new Chromosome();
            bestSolution.fitness = this.ChromosomePool[0].fitness;

            for (int i = 0; i < this.maxIterations; i++)
            {
                // Cross mating pairs and mutate them to form new population.
                this.CrossMatingPairs();
                //this.MutateNewPopulation(this.poolSize * ( 1 - this.avgDiversityPool / this.customers.Count));

                // Each 10 generations evaluate the population for insight into the pool sturucture.
                if (i % 10 == 0)
                {
                    //if (i % 20 == 0)
                    //    this.CustomerPositionFrequency();
                    this.LocalSearchImproveSolution();
                    this.GroupPopulationByFitness();
                    this.PopulationDiversityUpdate();

                    // If the average diversitiy is less than 60%, mutate the population.
                    if (this.avgDiversityPool < this.customers.Count * 0.60)
                    {
                        this.MutateNewPopulation(this.poolSize * (1 - this.avgDiversityPool / this.customers.Count));
                    }
                }

                // If multiple chromosomes are the same, mutate them until there is no duplicates present in the pool.
                if (this.customers.Count >= 7)
                {
                    int counter = 0;
                    var groups = this.ChromosomePool.GroupBy(x => x.fitness).Where(x => x.Count() > 1).ToList();
                    // If multiple solutions are same, mutate them.
                    while (groups.Count > 0 && counter <= 2)
                    {
                        foreach (var group in groups)
                        {
                            var tempGroup = group.ToList();
                            for (int j = 0; j < tempGroup.Count(); j++)
                            {
                                // Dont mutate the first solution in the series.
                                if (j == 0)
                                    continue;

                                while (tempGroup[j].fitness == group.Key)
                                {
                                    this.Mutation(tempGroup[j]);
                                    this.Mutation(tempGroup[j]);
                                    tempGroup[j].fitness = this.Fitness(tempGroup[j]);
                                }
                            }
                        }
                        groups = this.ChromosomePool.GroupBy(x => x.fitness).Where(x => x.Count() > 1).ToList();
                        counter++;
                    }
                }

                // Select top n solutions from the population. Select by fitness.
                this.ChromosomePool = this.ChromosomePool.Select(x => x).OrderBy(x => x.fitness).Take(this.poolSize).ToList();

                // If any of the elite Chromosomes is not in the new generation pool, add them.
                foreach (KeyValuePair<int, List<Chromosome>> eliteGroup in this.eliteChromosome)
                {
                    foreach (Chromosome member in eliteGroup.Value)
                    {
                        if (!this.ChromosomePool.Contains(member))
                            this.ChromosomePool.Add(member);
                    }
                }

                if (bestSolution.fitness <= this.ChromosomePool[0].fitness)
                {
                    generationsWithoutBestFitImprovement++;
                }
                else
                {
                    generationsWithoutBestFitImprovement = 0;
                    Chromosome temp = this.ChromosomePool[0];
                    bestSolution.fitness = temp.fitness;
                    bestSolution.genes = new List<Vertex>(temp.genes);
                    this.solutionImprovedCount++;
                }

                // If best solution did not improve x iterations, diversify the population by
                // preserving top 20% solutions from each population group and fill rest of
                // the population with new individuals as in the initialization phase.
                // This process aims to introduce a significant amount of new Genetic material.
                if (generationsWithoutBestFitImprovement == this.maxBestNotImproved * 0.4)
                {
                    this.GlobalDiversificationOfPopulation();
                    this.PopulationDiversityUpdate();
                }

                if (generationsWithoutBestFitImprovement == this.maxBestNotImproved)
                    break;

                this.GroupPopulationByFitness();
                this.MatingPoolSelection();

                this.iterationCount++;
            }

            bestSolution.genes.Insert(0, this.graph.vertices[0]);
            bestSolution.genes.Add(this.graph.vertices[0]);
            return bestSolution.genes;
        }
    }
}
