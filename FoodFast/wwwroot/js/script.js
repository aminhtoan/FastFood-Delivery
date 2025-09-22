




// Bootstrap library
const bootstrap = window.bootstrap

// Initialize the app
document.addEventListener("DOMContentLoaded", () => {
  renderDishes()
  updateCartUI()
  setupEventListeners()
})

// Setup event listeners
function setupEventListeners() {
  // Category filter
  const categoryCards = document.querySelectorAll(".category-card")
  categoryCards.forEach((card) => {
    card.addEventListener("click", function () {
      const category = this.dataset.category
      filterDishes(category)
    })
  })

  // Contact form with Bootstrap validation
  const contactForm = document.querySelector(".contact-form")
  if (contactForm) {
    contactForm.addEventListener("submit", function (e) {
      e.preventDefault()

      if (this.checkValidity()) {
        showBootstrapToast("Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm nhất.", "success")
        this.reset()
        this.classList.remove("was-validated")
      } else {
        this.classList.add("was-validated")
      }
    })
  }

  

  /

  // Smooth scrolling for navigation links
 

// Render dishes with Bootstrap cards


// Filter dishes by category
function filterDishes(category) {
  const filteredDishes = dishes.filter((dish) => dish.category === category)
  renderDishes(filteredDishes)

  // Scroll to dishes section
  const popularSection = document.querySelector(".popular-dishes")
  if (popularSection) {
    popularSection.scrollIntoView({
      behavior: "smooth",
      block: "start",
    })
  }
}






// Format price
function formatPrice(price) {
  return new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
  }).format(price)
}

// Show Bootstrap toast
function showBootstrapToast(message, type = "info") {
  let toastContainer = document.querySelector(".toast-container")
  if (!toastContainer) {
    toastContainer = document.createElement("div")
    toastContainer.className = "toast-container position-fixed top-0 end-0 p-3"
    toastContainer.style.zIndex = "3000"
    document.body.appendChild(toastContainer)
  }

  const toastId = "toast-" + Date.now()
  const toastHTML = `
    <div id="${toastId}" class="toast align-items-center text-bg-${type} border-0" role="alert" aria-live="assertive" aria-atomic="true">
      <div class="d-flex">
        <div class="toast-body">
          ${message}
        </div>
        <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
      </div>
    </div>
  `

  toastContainer.insertAdjacentHTML("beforeend", toastHTML)

  const toastElement = document.getElementById(toastId)
  const toast = new bootstrap.Toast(toastElement, {
    autohide: true,
    delay: 3000,
  })

  toast.show()

  toastElement.addEventListener("hidden.bs.toast", () => {
    toastElement.remove()
  })
}

// Show order confirmation modal
function showOrderConfirmation(orderSummary, total) {
  let confirmModal = document.getElementById("orderConfirmModal")
  if (!confirmModal) {
    const modalHTML = `
      <div class="modal fade" id="orderConfirmModal" tabindex="-1" aria-labelledby="orderConfirmModalLabel" aria-hidden="true">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header bg-success text-white">
              <h5 class="modal-title" id="orderConfirmModalLabel">
                <i class="fas fa-check-circle me-2"></i>Đặt hàng thành công!
              </h5>
              <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
              <h6>Chi tiết đơn hàng:</h6>
              <div id="orderDetails" class="mb-3"></div>
              <hr>
              <div class="d-flex justify-content-between">
                <strong>Tổng cộng: </strong>
                <strong id="orderTotal" class="text-success"></strong>
              </div>
              <div class="alert alert-info mt-3 mb-0">
                <i class="fas fa-clock me-2"></i>Chúng tôi sẽ giao hàng trong 30 phút!
              </div>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-success" data-bs-dismiss="modal">Đóng</button>
            </div>
          </div>
        </div>
      </div>
    `
    document.body.insertAdjacentHTML("beforeend", modalHTML)
    confirmModal = document.getElementById("orderConfirmModal")
  }

  document.getElementById("orderDetails").innerHTML = orderSummary.replace(/\n/g, "<br>")
  document.getElementById("orderTotal").textContent = formatPrice(total)

  const modal = new bootstrap.Modal(confirmModal)
  modal.show()

  confirmModal.addEventListener(
    "hidden.bs.modal",
    () => {
      cart = []
      updateCartUI()
      const cartModal = bootstrap.Modal.getInstance(document.getElementById("cartModal"))
      if (cartModal) {
        cartModal.hide()
      }
    },
    { once: true },
  )
}

// Navbar scroll effect
window.addEventListener("scroll", () => {
  const navbar = document.querySelector(".navbar")
  if (navbar) {
    if (window.scrollY > 100) {
      navbar.classList.add("navbar-scrolled")
    } else {
      navbar.classList.remove("navbar-scrolled")
    }
  }
})
