
// Base API URL (Ensure it matches backend settings)
const API_BASE_URL = "http://localhost:5001/api";

// Global application state
let state = {
  token: localStorage.getItem("token") || null,
  user: null // will be populated after login
};

// Update navbar based on login state
function updateNavbar() {
  const userInfoElem = document.getElementById("user-info");
  const loginBtn = document.getElementById("login-btn");
  const logoutBtn = document.getElementById("logout-btn");
  const navMyRentals = document.getElementById("nav-myrentals");
  const navAdmin = document.getElementById("nav-admin");

  if (state.token && state.user) {
    userInfoElem.textContent = `${state.user.Username || state.user.username} (${state.user.Role || state.user.role})`;
    loginBtn.style.display = "none";
    logoutBtn.style.display = "inline-block";

    // Show "My Rentals" if the user type is filmstudio
    if ((state.user.Role || state.user.role).toLowerCase() === "filmstudio") {
      navMyRentals.style.display = "block";
    } else {
      navMyRentals.style.display = "none";
    }
    // Show admin dashboard if the user is admin
    if ((state.user.Role || state.user.role).toLowerCase() === "admin") {
      navAdmin.style.display = "block";
    } else {
      navAdmin.style.display = "none";
    }
  } else {
    userInfoElem.textContent = "";
    loginBtn.style.display = "inline-block";
    logoutBtn.style.display = "none";
    navMyRentals.style.display = "none";
    navAdmin.style.display = "none";
  }
}

// Function to show notifications (non-popup) inside the page
function showNotification(message, type = "success") {
  const notificationElem = document.getElementById("notification");
  notificationElem.textContent = message;
  notificationElem.style.backgroundColor = type === "success" ? "#28a745" : "#dc3545";
  notificationElem.style.display = "block";
  setTimeout(() => {
    notificationElem.style.display = "none";
  }, 3000);
}

// Helper function to call API with token if available
async function apiCall(endpoint, options = {}) {
  if (state.token) {
    options.headers = {
      ...options.headers,
      "Authorization": `Bearer ${state.token}`,
    };
  }
  const response = await fetch(`${API_BASE_URL}${endpoint}`, options);
  return response;
}

// Router function: load appropriate view based on hash value
function router() {
  const hash = window.location.hash || "#home";
  const mainContent = document.getElementById("main-content");
  mainContent.innerHTML = ""; // Clear current content

  if (hash.startsWith("#home")) {
    showHome();
  } else if (hash.startsWith("#login")) {
    showLogin();
  } else if (hash.startsWith("#register")) {
    showRegister();
  } else if (hash.startsWith("#film/")) {
    const filmId = hash.split("/")[1];
    showFilmDetail(filmId);
  } else if (hash.startsWith("#myrentals")) {
    showMyRentals();
  } else if (hash.startsWith("#admin")) {
    showAdminDashboard();
  } else {
    showHome();
  }
}

// Show home page: list of films
async function showHome() {
  const mainContent = document.getElementById("main-content");
  mainContent.innerHTML = ""; // Reset content
  const header = document.createElement("h2");
  header.textContent = "Available Films";
  mainContent.appendChild(header);

  try {
    const response = await apiCall("/films", { method: "GET" });
    if (!response.ok) {
      throw new Error("Error connecting to API");
    }
    const movies = await response.json();
    console.log("Data received from API:", movies);

    if (!Array.isArray(movies) || movies.length === 0) {
      mainContent.innerHTML += "<p>No films available at the moment.</p>";
      return;
    }
    
    // Determine user role if exists
    const role = state.user ? (state.user.Role || state.user.role).toLowerCase() : null;
    
    movies.forEach(movie => {
      const card = document.createElement("div");
      card.className = "film-card";

      const copies = movie.filmCopies || movie.FilmCopies;
      let copiesInfo = "";
      // Display copies info only if user is admin or filmstudio
      if (role === "admin" || role === "filmstudio") {
        const availableCount = copies && Array.isArray(copies)
            ? copies.filter(copy => (copy.isAvailable !== undefined ? copy.isAvailable : copy.IsAvailable)).length
            : "Not available";
        copiesInfo = `<p><strong>Available Copies:</strong> ${availableCount}</p>`;
      }

      card.innerHTML = `
        <h3>${movie.title || movie.Title}</h3>
        <div class="film-info">
          <p>${movie.description || movie.Description}</p>
          <p><strong>Director:</strong> ${movie.director || movie.Director}</p>
          <p><strong>Release Year:</strong> ${movie.releaseYear || movie.ReleaseYear}</p>
          ${copiesInfo}
        </div>
        <button onclick="window.location.hash='#film/${movie.filmId || movie.FilmId}'">View Details</button>
      `;
      mainContent.appendChild(card);
    });
  } catch (error) {
    showNotification("An error occurred while fetching films", "error");
    console.error("Error in showHome:", error);
  }
}

// Show login form
function showLogin() {
  const mainContent = document.getElementById("main-content");
  const container = document.createElement("div");
  container.className = "form-container";
  container.innerHTML = `
    <h2>Login</h2>
    <form id="login-form">
      <input type="text" id="login-username" placeholder="Username" required>
      <input type="password" id="login-password" placeholder="Password" required>
      <label><input type="checkbox" id="remember-me"> Remember me</label>
      <button type="submit">Login</button>
    </form>
    <p>Don't have an account? <a href="#register">Register Film Studio Account</a></p>
  `;
  mainContent.appendChild(container);

  const loginForm = document.getElementById("login-form");
  loginForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    const username = document.getElementById("login-username").value;
    const password = document.getElementById("login-password").value;
    try {
      const response = await fetch(`${API_BASE_URL}/users/authenticate`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ Username: username, Password: password })
      });
      if (response.ok) {
        const data = await response.json();
        state.token = data.Token || data.token;
        state.user = data;
        localStorage.setItem("token", state.token);
        showNotification("Successfully logged in");
        updateNavbar();
        window.location.hash = "#home";
      } else {
        const errMsg = await response.text();
        showNotification(errMsg, "error");
      }
    } catch (error) {
      showNotification("Error connecting to server", "error");
      console.error(error);
    }
  });
}

// Show Film Studio registration form
function showRegister() {
  const mainContent = document.getElementById("main-content");
  const container = document.createElement("div");
  container.className = "form-container";
  container.innerHTML = `
    <h2>Film Studio Registration</h2>
    <form id="register-form">
      <input type="text" id="register-name" placeholder="Studio Name" required>
      <input type="text" id="register-city" placeholder="City" required>
      <input type="text" id="register-username" placeholder="Username" required>
      <input type="password" id="register-password" placeholder="Password" required>
      <button type="submit">Register</button>
    </form>
    <p>Already have an account? <a href="#login">Login</a></p>
  `;
  mainContent.appendChild(container);

  const registerForm = document.getElementById("register-form");
  registerForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    const name = document.getElementById("register-name").value;
    const city = document.getElementById("register-city").value;
    const username = document.getElementById("register-username").value;
    const password = document.getElementById("register-password").value;
    try {
      const response = await fetch(`${API_BASE_URL}/filmstudio/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          Name: name,
          City: city,
          Username: username,
          Password: password
        })
      });
      if (response.ok) {
        showNotification("Registration successful, please login");
        window.location.hash = "#login";
      } else {
        const errMsg = await response.text();
        showNotification(errMsg, "error");
      }
    } catch (error) {
      showNotification("Error connecting to server", "error");
      console.error(error);
    }
  });
}

// Show film details with rental option (if user is filmstudio)
async function showFilmDetail(filmId) {
  const mainContent = document.getElementById("main-content");
  try {
    const response = await fetch(`${API_BASE_URL}/films/${filmId}`, {
      method: "GET",
      headers: state.token ? { "Authorization": `Bearer ${state.token}` } : {}
    });
    if (!response.ok) {
      showNotification("Film not found", "error");
      return;
    }
    const film = await response.json();
    const container = document.createElement("div");
    container.className = "film-detail";

    const copies = film.filmCopies || film.FilmCopies;
    let copiesInfo = "";
    const role = state.user ? (state.user.Role || state.user.role).toLowerCase() : null;
    if (role === "admin" || role === "filmstudio") {
      const availableCount = copies && Array.isArray(copies)
          ? copies.filter(copy => (copy.isAvailable !== undefined ? copy.isAvailable : copy.IsAvailable)).length
          : "Not available";
      copiesInfo = `<p><strong>Available Copies:</strong> ${availableCount}</p>`;
    }

    container.innerHTML = `
      <h2>${film.title || film.Title}</h2>
      <p>${film.description || film.Description}</p>
      <p><strong>Director:</strong> ${film.director || film.Director}</p>
      <p><strong>Release Year:</strong> ${film.releaseYear || film.ReleaseYear}</p>
      ${copiesInfo}
    `;

    // If the user is a FilmStudio, display the rental form
    if (role === "filmstudio") {
      const rentalForm = document.createElement("form");
      rentalForm.id = "rental-form";
      rentalForm.innerHTML = `
        <h3>Rent Film</h3>
        <label>Rental Start Date: <input type="date" id="rental-start" required></label>
        <label>Rental End Date: <input type="date" id="rental-end" required></label>
        <button type="submit">Rent</button>
      `;
      container.appendChild(rentalForm);

      rentalForm.addEventListener("submit", async (e) => {
        e.preventDefault();
        const rentalStart = document.getElementById("rental-start").value;
        const rentalEnd = document.getElementById("rental-end").value;
        
        const studioId = state.user.FilmStudioId || state.user.filmStudioId;
        if (!studioId) {
          showNotification("Film studio ID not available", "error");
          return;
        }
        
        try {
          const rentResponse = await apiCall(
            `/films/rent?id=${film.filmId || film.FilmId}&studioid=${studioId}&rentalStart=${rentalStart}&rentalEnd=${rentalEnd}`,
            {
              method: "POST",
              headers: { "Content-Type": "application/json" }
            }
          );
          if (rentResponse.ok) {
            showNotification("Film rented successfully");
            showFilmDetail(film.filmId || film.FilmId);
          } else {
            const errMsg = await rentResponse.text();
            showNotification(errMsg, "error");
          }
        } catch (error) {
          showNotification("Error while renting film", "error");
          console.error(error);
        }
      });
    }

    mainContent.appendChild(container);
  } catch (error) {
    showNotification("Error fetching film details", "error");
    console.error(error);
  }
}

// Show "My Rentals" page for filmstudio user
async function showMyRentals() {
  const mainContent = document.getElementById("main-content");
  mainContent.innerHTML = "";
  const header = document.createElement("h2");
  header.textContent = "My Rentals";
  mainContent.appendChild(header);

  try {
    const response = await apiCall("/mystudio/rentals", { method: "GET" });
    console.log("Response status:", response.status);
    const rentals = await response.json();
    console.log("Data from rentals endpoint:", rentals);
    
    if (!Array.isArray(rentals) || rentals.length === 0) {
      mainContent.innerHTML += "<p>No current rentals available.</p>";
      return;
    }
    rentals.forEach(rental => {
      if (!rental.film) {
        console.warn("No film data in this rental:", rental);
        return;
      }
      const filmId = rental.film.filmId || rental.film.FilmId;
      const card = document.createElement("div");
      card.className = "film-card";
      card.innerHTML = `
        <h3>${rental.film.title || rental.film.Title || "Title not available"}</h3>
        <p><strong>Rental Start Date:</strong> ${rental.rentalStartDate ? new Date(rental.rentalStartDate).toLocaleDateString() : "Not available"}</p>
        <p><strong>Rental End Date:</strong> ${rental.rentalEndDate ? new Date(rental.rentalEndDate).toLocaleDateString() : "Not available"}</p>
        <button onclick="returnFilm(${filmId})">Return Film</button>
      `;
      mainContent.appendChild(card);
    });
  } catch (error) {
    showNotification("Error fetching rentals", "error");
    console.error(error);
  }
}

// Function to return film (for filmstudio)
async function returnFilm(filmId) {
  try {
    const response = await apiCall(`/films/return?id=${filmId}&studioid=${state.user.FilmStudioId}`, {
      method: "POST"
    });
    if (response.ok) {
      showNotification("Film returned successfully");
      showMyRentals();
    } else {
      const errMsg = await response.text();
      showNotification(errMsg, "error");
    }
  } catch (error) {
    showNotification("Error while returning film", "error");
    console.error(error);
  }
}

// Show admin dashboard (functions for add, edit, delete)
async function showAdminDashboard() {
  if (!state.token || !(state.user && (state.user.Role || state.user.role).toLowerCase() === "admin")) {
    showNotification("You are not authorized to access this page", "error");
    window.location.hash = "#home";
    return;
  }

  const mainContent = document.getElementById("main-content");
  mainContent.innerHTML = "";
  const header = document.createElement("h2");
  header.textContent = "Admin Dashboard";
  mainContent.appendChild(header);

  // Form for adding a new film with cover image upload
  const addFilmContainer = document.createElement("div");
  addFilmContainer.className = "form-container";
  addFilmContainer.innerHTML = `
    <h3>Add New Film</h3>
    <form id="add-film-form" enctype="multipart/form-data">
      <input type="text" id="film-title" placeholder="Film Title" required>
      <textarea id="film-description" placeholder="Film Description" required></textarea>
      <input type="text" id="film-director" placeholder="Director" required>
      <input type="number" id="film-releaseYear" placeholder="Release Year" required>
      <input type="number" id="film-copies" placeholder="Number of Copies" required>
      <input type="file" id="film-cover" required>
      <button type="submit">Add Film</button>
    </form>
  `;
  mainContent.appendChild(addFilmContainer);

  document.getElementById("add-film-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const title = document.getElementById("film-title").value;
    const description = document.getElementById("film-description").value;
    const director = document.getElementById("film-director").value;
    const releaseYear = document.getElementById("film-releaseYear").value;
    const numberOfCopies = document.getElementById("film-copies").value;
    const coverFile = document.getElementById("film-cover").files[0];

    // Use FormData to upload data including the image
    const formData = new FormData();
    formData.append("Title", title);
    formData.append("Description", description);
    formData.append("Director", director);
    formData.append("ReleaseYear", releaseYear);
    formData.append("NumberOfCopies", numberOfCopies);
    formData.append("CoverImage", coverFile);

    try {
      const response = await fetch(`${API_BASE_URL}/films`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${state.token}`
          // Do not set Content-Type, the browser will set it with boundary
        },
        body: formData
      });
      if (response.ok) {
        showNotification("Film added successfully");
        loadAdminFilms();
      } else {
        const errMsg = await response.text();
        showNotification(errMsg, "error");
      }
    } catch (error) {
      showNotification("Error adding film", "error");
      console.error(error);
    }
  });

  // Area to display film list with edit and delete options
  const adminFilmsContainer = document.createElement("div");
  adminFilmsContainer.id = "admin-films-container";
  mainContent.appendChild(adminFilmsContainer);

  loadAdminFilms();
}

// Load film list in admin dashboard
async function loadAdminFilms() {
  const container = document.getElementById("admin-films-container");
  container.innerHTML = "<h3>Film List</h3>";
  try {
    const response = await apiCall("/films", { method: "GET" });
    const films = await response.json();
    films.forEach(film => {
      const filmElem = document.createElement("div");
      filmElem.className = "film-card";

      const copies = film.filmCopies || film.FilmCopies;
      const availableCount = copies && Array.isArray(copies)
          ? copies.filter(copy => (copy.isAvailable !== undefined ? copy.isAvailable : copy.IsAvailable)).length
          : "Not available";

      filmElem.innerHTML = `
        <h3>${film.title || film.Title}</h3>
        <p><strong>Director:</strong> ${film.director || film.Director}</p>
        <p><strong>Release Year:</strong> ${film.releaseYear || film.ReleaseYear}</p>
        <p><strong>Available Copies:</strong> ${availableCount}</p>
        <button onclick="editFilm(${film.filmId || film.FilmId})">Edit</button>
        <button onclick="deleteFilm(${film.filmId || film.FilmId})">Delete</button>
      `;
      container.appendChild(filmElem);
    });
  } catch (error) {
    showNotification("Error fetching film list", "error");
    console.error(error);
  }
}

// Function to edit film (display preliminary edit form)
async function editFilm(filmId) {
  try {
    const response = await apiCall(`/films/${filmId}`, { method: "GET" });
    if (!response.ok) {
      showNotification("Unable to fetch film data for editing", "error");
      return;
    }
    const film = await response.json();
    const container = document.getElementById("admin-films-container");
    container.innerHTML = `
      <div class="form-container">
        <h3>Edit Film</h3>
        <form id="edit-film-form">
          <input type="text" id="edit-film-title" placeholder="Film Title" value="${film.title || film.Title}" required>
          <textarea id="edit-film-description" placeholder="Film Description" required>${film.description || film.Description}</textarea>
          <input type="text" id="edit-film-director" placeholder="Director" value="${film.director || film.Director}" required>
          <input type="number" id="edit-film-releaseYear" placeholder="Release Year" value="${film.releaseYear || film.ReleaseYear}" required>
          <input type="number" id="edit-film-copies" placeholder="Number of Copies" value="${film.filmCopies ? film.filmCopies.length : 0}" required>
          <button type="submit">Save Changes</button>
          <button type="button" onclick="loadAdminFilms()">Cancel</button>
        </form>
      </div>
    `;
    document.getElementById("edit-film-form").addEventListener("submit", async (e) => {
      e.preventDefault();
      const title = document.getElementById("edit-film-title").value;
      const description = document.getElementById("edit-film-description").value;
      const director = document.getElementById("edit-film-director").value;
      const releaseYear = document.getElementById("edit-film-releaseYear").value;
      const numberOfCopies = document.getElementById("edit-film-copies").value;
      try {
        const updateResponse = await fetch(`${API_BASE_URL}/films/${filmId}`, {
          method: "PATCH",
          headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${state.token}`
          },
          body: JSON.stringify({
            Title: title,
            Description: description,
            Director: director,
            ReleaseYear: parseInt(releaseYear),
            NumberOfCopies: parseInt(numberOfCopies)
          })
        });
        if (updateResponse.ok) {
          showNotification("Film data updated successfully");
          loadAdminFilms();
        } else {
          const errMsg = await updateResponse.text();
          showNotification(errMsg, "error");
        }
      } catch (error) {
        showNotification("Error updating film", "error");
        console.error(error);
      }
    });
  } catch (error) {
    showNotification("Error fetching film data", "error");
    console.error(error);
  }
}

// Function to delete film
async function deleteFilm(filmId) {
  if (!confirm("Are you sure you want to delete the film?")) return;
  try {
    const response = await fetch(`${API_BASE_URL}/films/${filmId}`, {
      method: "DELETE",
      headers: {
        "Authorization": `Bearer ${state.token}`
      }
    });
    if (response.ok) {
      showNotification("Film deleted successfully");
      loadAdminFilms();
    } else {
      const errMsg = await response.text();
      showNotification(errMsg, "error");
    }
  } catch (error) {
    showNotification("Error deleting film", "error");
    console.error(error);
  }
}

// Logout function
function logout() {
  state.token = null;
  state.user = null;
  localStorage.removeItem("token");
  updateNavbar();
  showNotification("Logged out successfully");
  window.location.hash = "#home";
}

// Initialization function and listen for hash changes
function init() {
  updateNavbar();
  router();

  if (state.token && !state.user) {
    // You can add an API call here to fetch user data if needed.
  }
}

window.addEventListener("hashchange", router);
window.addEventListener("load", init);

// Navbar button events
document.getElementById("login-btn").addEventListener("click", () => {
  window.location.hash = "#login";
});
document.getElementById("logout-btn").addEventListener("click", logout);
