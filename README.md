
# Tresorit Demo
A basic product commenting demo that uses Azure Table Storage.
## Functions
There are two ways to interact with the service. You can either fetch some comments or post a single comment for a product.
### GetComments
Returns comments for the specified Product in a JSON list orderd by time. It has two parameters:
 - `product`: name of the desired product
 - `limit`: number of comments to fetch

#### Example call: 
`http://localhost:8000/GetComments?product=Laptop&limit=6`

This returns the 6 most recent comments for the product `Laptop`.
### PostComment
Posts comment for a product.
The parameters should be specified in the body of the message:

 - Comment: the text of the comment
 - Product: the name of the product
 - CommentID: ID of the **latest** comment for this product
 - Username: any name you want your comment to be registered with

#### Example call: 
`http://localhost:8000/PostComment`

with the body of:

    {
    "Comment": "Great product, I recommend it!",
    "Product": "Laptop",
    "CommentID": "f41cd228-f145-462b-8f2d-3192918e0c81",
    "Username": "John"
    }

## Requirements

Before running the service make sure that a local instance of Microsoft Azure Storage Emulator is running.

## Explanation
  - The way the service prevents the user from posting a comment before the most recent comment appears is that it requires to include the GUID of that last comment in the newly posted comment JSON's body.
