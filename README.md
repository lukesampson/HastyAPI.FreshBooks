Overview
========

This is a .NET library for interacting with the FreshBooks API.

It's __low-level__ and __dynamic__, making it flexible and expressive. The downside is, you'll need to refer to the [FreshBooks API](http://developers.freshbooks.com) in order to use it (no help from intellisense!).

Examples
========

Example: basic request and response
-----------------------------------
Getting a [list of all your clients](http://developers.freshbooks.com/docs/clients/#client.list])

    var fb = new FreshBooks("yourusername", "yourapitoken");
	var result = fb.Call("client.list");

	// note: 'result' is dynamic, converted from the XML returned by FreshBooks
	// so you can do, e.g.:
	var total = result.response.clients.total;

Example: specifying request parameters
--------------------------------------
Searching for clients updated in the last week:

    var fb = new FreshBooks("yourusername", "yourapitoken");
	var result = fb.Call("client.list", x => x.updated_from = DateTime.Now.AddDays(-7));

	// e.g. get the first client's email address
	var email = result.response.clients.client[0].email;

Example: multiple request parameters
------------------------------------
Searching for unpaid invoices from the last month:

	var fb = new FreshBooks("yourusername", "yourapitoken");
	var unpaid = fb.Call("invoice.list", x => {
		x.date_from = DateTime.Now.AddMonths(-1);
		x.status = "unpaid";
	});

Example: inspecting data returned from the API
----------------------------------------------
You can view the data by calling ToString() on the dynamic result:

	var fb = new FreshBooks("yourusername", "yourapitoken");
	var expense = fb.Call("expense.get", x => x.expense_id = 433);

	// this will output the contents of the response
	Console.WriteLine(expense.ToString());