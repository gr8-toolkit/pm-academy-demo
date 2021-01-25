
**Use PLINQ**

if your operation requires the output to preserve the ordering of the input data set

processing the query output as a stream
> elements are distributed into partitions, processed with multiple threads and then
rearranged, then single threaded consumer in the loop can begin its computation

operating over two collections
