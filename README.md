# AIRecommender

### Problem Statement
Create an artificial intelligence recommendation system by employing the Pearson correlation coefficient machine learning algorithm by analysing and implementating the given high-level design.<br>

#### Step 1 - Recommender
![image](https://github.com/Sanjanabharadwaj25/AIRecommender/assets/77878089/3f766abf-bcdd-4770-a3dd-b88c72a59c84)<br>
#### Approach<br>
It includes interfaces for the recommender and data preprocessor, where the data preprocessor standardizes and handles missing values in input arrays. The PearsonRecommender class calculates the correlation between two datasets, applying preprocessing, and addressing scenarios where the denominator becomes zero. The RecommenderFactory facilitates the creation of a recommendation system instance with a default DataPreprocessor. <br>
#### Step 2 - Data Loader
![image](https://github.com/Sanjanabharadwaj25/AIRecommender/assets/77878089/6c479ce3-8e45-4844-858e-a95ab498d348)<br>
#### Approach<br>
Defining a data loader system for processing information from CSV files containing details about books, users, and user ratings. The BookDetails class encapsulates dictionaries and lists to store information about books, users, and their ratings. The IDataLoader interface outlines the method for loading data, and the CSVDataLoader class implements this interface, utilizing asynchronous tasks to concurrently load information from separate CSV files into the BookDetails instance. <br>
#### Step 3 - Data Aggregator
![image](https://github.com/Sanjanabharadwaj25/AIRecommender/assets/77878089/9a18e11a-bc50-4c6f-a473-e87ef7239a6f)<br>
#### Approach<br>
Defining a ratings aggregator module that, given a set of user preferences (state, age), aggregates book ratings from a BookDetails instance based on user demographics. The RatingsAggregator class implements the IRatingsAggregator interface, processing user data and filtering book ratings according to specified criteria. The aggregation is performed by categorizing users into age groups and checking for matching states, then collecting and organizing book ratings for each corresponding ISBN. <br>
#### Step 4 - UI
![image](https://github.com/Sanjanabharadwaj25/AIRecommender/assets/77878089/e2c5d16f-8c7f-4fdd-89aa-d380ed914ebb)<br>
#### Approach<br>
Represents an AI-based recommendation system for books. It integrates various components, including a DataLoader for loading book, user, and rating data from CSV files, a RatingsAggregator for aggregating user ratings based on demographic preferences, and a Recommender for calculating correlations using the Pearson algorithm. The AIRecommendationEngine orchestrates these components to generate book recommendations for a given user preference, incorporating caching using MemDataCacher. The program demonstrates modularity, encapsulation, and efficiency by leveraging interfaces and a factory method for recommender creation. The execution in the Program class showcases the functionality by allowing a user to input their book preference, state, and age, followed by the system generating and displaying book recommendations along with the elapsed time for the recommendation process.<br>
#### Step 5 - Caching
![image](https://github.com/Sanjanabharadwaj25/AIRecommender/assets/77878089/924c057b-a0fc-4c0b-b5ea-1167532dc5f5)<br>
#### Approach<br>
Defining a caching mechanism for the AI recommendation system using Redis through the MemDataCacher class, implementing the IDataCacher interface. The MemDataCacher utilizes the StackExchange.Redis library to connect to Redis, providing methods to retrieve and store BookDetails data based on a specified key. The SetData method serializes the data into JSON format and sets it in the Redis cache with a predefined expiration time.


