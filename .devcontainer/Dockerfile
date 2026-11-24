FROM mcr.microsoft.com/dotnet/sdk:10.0

ARG USERNAME=vscode
ARG USER_UID=1001
ARG USER_GID=$USER_UID

ENV DEBIAN_FRONTEND=noninteractive \
    TMPDIR=/tmp \
    DOTNET_CLI_HOME=/home/vscode \
    HOME=/home/vscode \
    VIRTUAL_ENV=/opt/venv
ENV PATH="$VIRTUAL_ENV/bin:$PATH"

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ffmpeg \
        git \
        python3 \
        python3-pip \
        python3-venv \
        sudo \
    && rm -rf /var/lib/apt/lists/*

RUN if ! getent group $USERNAME >/dev/null; then \
        groupadd --gid $USER_GID $USERNAME; \
    fi \
    && if ! id -u $USERNAME >/dev/null 2>&1; then \
        useradd --uid $USER_UID --gid $USER_GID --create-home --shell /bin/bash $USERNAME; \
    fi \
    && echo "$USERNAME ALL=(root) NOPASSWD:ALL" >/etc/sudoers.d/$USERNAME \
    && chmod 0440 /etc/sudoers.d/$USERNAME

RUN python3 -m venv $VIRTUAL_ENV \
    && pip install --upgrade pip \
    && pip install faster-whisper \
    && chown -R $USERNAME:$USERNAME $VIRTUAL_ENV \
    && mkdir -p \
        /home/$USERNAME/.nuget/packages \
        /home/$USERNAME/.cache/huggingface \
    && chown -R $USERNAME:$USERNAME \
        /home/$USERNAME/.nuget \
        /home/$USERNAME/.cache

USER $USERNAME
WORKDIR /workspaces/nyx-scriptorium
