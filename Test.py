import pyarrow.plasma as plasma
import numpy as np
import pyarrow as pa
import pandas as pd

client = plasma.connect("/tmp/plasma")

print(client.list())

object_id = plasma.ObjectID(20 * b"a")

# Fetch the Plasma object
[data] = client.get_buffers([object_id])  # Get PlasmaBuffer from ObjectID
buffer = pa.BufferReader(data)

# Convert object back into an Arrow RecordBatch
reader = pa.RecordBatchStreamReader(buffer)
record_batch = reader.read_next_batch()

# Convert back into Pandas
result = record_batch.to_pandas()

print(result.shape)

exit()

# Create a Pandas DataFrame
d = {'one' : pd.Series([1., 2., 3.], index=['a', 'b', 'c']),
     'two' : pd.Series([1., 2., 3., 4.], index=['a', 'b', 'c', 'd'])}
df = pd.DataFrame(d)

# Convert the Pandas DataFrame into a PyArrow RecordBatch
record_batch = pa.RecordBatch.from_pandas(df)

# Create the Plasma object from the PyArrow RecordBatch. Most of the work here
# is done to determine the size of buffer to request from the object store.
object_id = plasma.ObjectID(np.random.bytes(20))
mock_sink = pa.MockOutputStream()
stream_writer = pa.RecordBatchStreamWriter(mock_sink, record_batch.schema)
stream_writer.write_batch(record_batch)
stream_writer.close()
data_size = mock_sink.size()
buf = client.create(object_id, data_size)

# Write the PyArrow RecordBatch to Plasma
stream = pa.FixedSizeBufferWriter(buf)
stream_writer = pa.RecordBatchStreamWriter(stream, record_batch.schema)
stream_writer.write_batch(record_batch)
stream_writer.close()

# Seal the Plasma object
client.seal(object_id)

print(client.list())