# MovieTicketBookingSystem

A production-grade multithreaded movie ticket booking system in **.NET 8** (C#), designed to handle 1000+ concurrent users. This project demonstrates real-world concurrency control, thread safety, race condition prevention, and performance validation — ideal for interview showcases or tech portfolios.

---

## 🚀 Key Features

* ✅ **Multithreaded Booking Engine** using `Parallel.ForEachAsync`
* 🧠 **Concurrency-safe seat allocation** for multiple movies
* 🚫 **Race condition & deadlock prevention**
* ⛔ Prevents overbooking or duplicate seat assignment
* ⏱️ **Latency monitoring** with `Stopwatch`
* 🔁 **Simulates 1000+ users** booking tickets simultaneously
* 🧪 **Robust Unit Testing Suite** with full coverage and performance benchmarks

---

## 🛠️ Technologies Used

* **.NET 8 / C#**
* **JetBrains Rider** (cross-platform)
* **xUnit** for testing
* **Ubuntu 24.04** as development OS

---

## 📁 Folder Structure

```
MovieTicketBookingSystem/
├── src/
│   └── MovieBooking/
│       ├── Program.cs
│       ├── Models/
│           ├── Booking.cs
│           ├── Movie.cs
│           └── User.cs
│       ├── Orchestrator
│           └── BookingOrchestrator.cs
│       └── Services/
│           ├── BookingService.cs
│           └── DeadlockBookingService.cs
├── tests/
│   └── MovieBooking.Tests/
│       └── BookingServiceTests.cs
├── .gitignore
├── README.md
└── MovieTicketBookingSystem.sln
```

---

## 🧪 Unit Test Coverage

| Test Case                                       | Status |
| ----------------------------------------------- | ------ |
| Booking should be thread-safe                   | ✅      |
| All seat numbers should be unique               | ✅      |
| Prevent overbooking                             | ✅      |
| No duplicate seat assignment                    | ✅      |
| Booking fails gracefully if seats are exhausted | ✅      |
| Handles 1000+ concurrent users                  | ✅      |
| Verifies booking latency                        | ✅      |
| Repeatable, deterministic test under stress     | ✅      |

Run tests:

```bash
cd tests/MovieBooking.Tests
dotnet test --logger:"console;verbosity=detailed"
```

---

## 🔧 Git Setup Notes

* ✅ `.gitignore` is in place
* ❗ No `.editorconfig` yet (recommended to enforce consistent coding style)
* CI/CD: GitHub Actions pipeline setup pending (optional)


---

## 👤 Author

**Nikunj Katariya**

* GitHub: [@nikunjkatariya](https://github.com/nikunjkatariya)

---

## 💡 Want to Contribute?

Not yet open for external contributions, but feel free to fork it and explore improvements — or ping the author for ideas!

---

## 💬 Contact

For feedback, improvements, or collaboration — reach out via GitHub issues or discussions.

---

### ⭐ If you like this project, consider giving it a star!
