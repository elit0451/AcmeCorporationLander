# AcmeCorporationWebApplication

 

A web application for drawing prizes from “Acme Corporation”. A person can enter a serial number of an existing product, if they are above 18 years old to make a submission and they can draw maximum twice a product with valid serial number.

In order to start the project, the first thing you need to do after you clone it is to execute the “execute_db.sql” in an SQL Server instance and the entire database will be set up for you (this includes the creation of the tables, stored procedures and some pre-filled data). Then, you should change the connection string to the one you executed the queries on.

The application solution is composed out of 3 projects – a web application project, a class library for keeping the model independent from the views and the logic in the controller, and a unit test project for verifying the data in the submission form.

When you run the application you are being presented with a list with all the submissions (they are records taken from the database) and if you select to make a “New submission” by clicking the link right under the “List of submissions” title, this will lead you to a page where you can input the required information and if everything is correct (you are a registered user and your age is above 18; you haven’t requested a product more than twice) your form will be submitted and added to the list. The list is displaying 10 submissions at a time.

If you want to check the registered customers and their relative age you can check the following records, where the last attribute is their age and a customer is identified by their email address.
```
('John', 'Andersen', 'j.andersen@gmail.com', 45)
('Elena', 'Tomsen', 'elena.t@gmail.com', 27)
('Isabella', 'Smith', 'isa.smith@gmail.com', 16)
('Anders', 'Bensen', 'anders.bensen@gmail.com', 18)
('Frederik', 'Beck', 'fred.beck@gmail.com', 30)
('Simon', 'Smith', 'simba_smith@gmail.com', 50)
('Louise', 'Grill', 'l.grill@gmail.com', 33)
('Emily', 'Bensen', 'emilybensen@gmail.com', 15)
('Lara', 'Morten', 'lara.morten@gmail.com', 23)
('Ben', 'Nikolovich', 'b.nikolovich@gmail.com', 55)
('Robert', 'Peterson', 'robby.peterson@gmail.com', 31)
('Ashley', 'Camelson', 'a1970camelson@gmail.com', 48)
('Jim', 'Kolev', 'jim_kolev@gmail.com', 19)
```
