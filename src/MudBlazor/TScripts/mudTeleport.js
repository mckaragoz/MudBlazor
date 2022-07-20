window.mudTeleport = {
  teleport: (source, to) => {
    const target = document.querySelector(to);
    if (!target) {
      throw new Error(`teleport: ${to} is not found on the DOM`);
    }
    target.appendChild(source);
  },

  removeFromDOM: (el) => {
    if (el && el.__internalId !== null) el.remove();
  },
};
