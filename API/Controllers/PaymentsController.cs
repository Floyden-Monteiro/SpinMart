using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly IGenericRepository<Payment> _paymentRepo;

        public PaymentsController(IPaymentService paymentService, IGenericRepository<Payment> paymentRepo)
        {
            _paymentService = paymentService;
            _paymentRepo = paymentRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Payment>>> GetPayments()
        {
            var payments = await _paymentRepo.ListAllAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _paymentRepo.GetByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<Payment>> GetPaymentByOrderId(int orderId)
        {
            var payment = await _paymentService.GetPaymentByOrderIdAsync(orderId);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> ProcessPayment([FromBody] Payment payment)
        {
            if (payment.Amount <= 0)
                return BadRequest("Payment amount must be greater than zero");

            var result = await _paymentService.ProcessPaymentAsync(payment);
            if (result == null)
                return BadRequest("Payment processing failed");

            return CreatedAtAction(nameof(GetPayment), new { id = result.Id }, result);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdatePaymentStatus(int id, [FromBody] PaymentStatus status)
        {
            var result = await _paymentService.UpdatePaymentStatusAsync(id, status);
            if (!result) return BadRequest("Failed to update payment status");
            return Ok();
        }

        [HttpGet("methods")]
        public ActionResult<IEnumerable<string>> GetPaymentMethods()
        {
            var methods = Enum.GetNames(typeof(PaymentMethod));
            return Ok(methods);
        }

        [HttpGet("statuses")]
        public ActionResult<IEnumerable<string>> GetPaymentStatuses()
        {
            var statuses = Enum.GetNames(typeof(PaymentStatus));
            return Ok(statuses);
        }

        [HttpGet("summary")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaymentSummaryDto>> GetPaymentSummary()
        {
            var payments = await _paymentRepo.ListAllAsync();
            var summary = new PaymentSummaryDto
            {
                TotalPayments = payments.Count(),
                TotalAmount = payments.Sum(p => p.Amount),
                CompletedPayments = payments.Count(p => p.Status == PaymentStatus.Completed),
                PendingPayments = payments.Count(p => p.Status == PaymentStatus.Pending)
            };
            return Ok(summary);
        }
    }
}